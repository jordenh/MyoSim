#include "stdafx.h"
#include "HubSim.hpp"
#include "DeviceListenerSim.h"
#include <string>
#include <iostream>
#include <map>
#include <cfloat>

#define PIPE_COMM_HEADER_SIZE 3
#define SYNC_DATA_NUM_BYTES 40
#define SYNC_DATA_NUM_FLOATS 10
#define POSE_DATA_NUM_BYTES 2
#define ARM_RECOGNIZED_DATA_NUM_BYTES 2

using namespace myoSim;

std::map<EventType, myoSimEvent> Hub::eventTypeToEventMap = {
        { ET_PAIRED, myoSimEvent::paired },
        { ET_UNPAIRED, myoSimEvent::unpaired },
        { ET_CONNECTED, myoSimEvent::connected },
        { ET_DISCONNECTED, myoSimEvent::disconnected },
        { ET_ARM_RECOGNIZED, myoSimEvent::armRecognized },
        { ET_ARM_LOST, myoSimEvent::armLost },
        { ET_SYNC_DAT, myoSimEvent::orientation },
        { ET_POSE, myoSimEvent::pose }
};

Hub::Hub(const std::string& applicationIdentifier)
{
    // Do nothing
}

Hub::~Hub()
{
    // Do nothing
}

Myo* Hub::waitForMyo(unsigned int milliseconds)
{
    // For now, only support one simulated myo (one-to-one connection to GUI). 
    // Look into changing this later, leave as a vector of myos.
    if (myos.size()) return NULL;

    Myo* mockMyo = new Myo(myos.size());
    if (!mockMyo->connectToPipe(milliseconds)) return NULL;
    
    myos.push_back(mockMyo);
    return mockMyo;
}

void Hub::addListener(DeviceListener* listener)
{
    if (std::find(listeners.begin(), listeners.end(), listener) != listeners.end()) {
        // Listener was already added.
        return;
    }

    listeners.push_back(listener);
}

void Hub::removeListener(DeviceListener* listener)
{
    std::vector<DeviceListener*>::iterator it = std::find(listeners.begin(), listeners.end(), listener);

    if (it == listeners.end()) {
        // Listener is not in list.
        return;
    }

    listeners.erase(it);
}

void Hub::onDeviceEvent(SimEvent simEvent)
{
    std::vector<DeviceListener*>::iterator it;
    uint64_t timestamp = simEvent.getEventTimestamp();

    Myo* myo = lookupMyo(simEvent.getMyoIdentifier());

    if (!myo)
    {
        return;
    }

    FirmwareVersion version;
    version.firmwareVersionHardwareRev = 1;
    version.firmwareVersionMajor = 1;
    version.firmwareVersionMinor = 0;
    version.firmwareVersionPatch = 1;

    for (it = listeners.begin(); it != listeners.end(); it++)
    {
        DeviceListener* listener = *it;

        switch (simEvent.getEventType())
        {
        case myoSimEvent::paired:
            listener->onPair(myo, timestamp, version);
            break;
        case myoSimEvent::unpaired:
            listener->onUnpair(myo, timestamp);
            break;
        case myoSimEvent::connected:
            listener->onConnect(myo, timestamp, version);
            break;
        case myoSimEvent::disconnected:
            listener->onDisconnect(myo, timestamp);
            break;
        case myoSimEvent::armRecognized:
            listener->onArmRecognized(myo, timestamp, simEvent.getArm(), simEvent.getXDirection());
            break;
        case myoSimEvent::armLost:
            listener->onArmLost(myo, timestamp);
            break;
        case myoSimEvent::orientation:
        {
            myo::Quaternion<float> orientation = myo::Quaternion<float>(simEvent.getXOrientation(),
                simEvent.getYOrientation(), simEvent.getZOrientation(), simEvent.getWOrientation());
            listener->onOrientationData(myo, timestamp, orientation);

            myo::Vector3<float> accelerometerData = myo::Vector3<float>(simEvent.getAccelerometerX(),
                simEvent.getAccelerometerY(), simEvent.getAccelerometerZ());
            listener->onAccelerometerData(myo, timestamp, accelerometerData);

            myo::Vector3<float> gyroData = myo::Vector3<float>(simEvent.getGyroscopeYawPerSecond(),
                simEvent.getGyroscopePitchPerSecond(), simEvent.getGyroscopeRollPerSecond());
            listener->onGyroscopeData(myo, timestamp, gyroData);
            break;
        }
        case myoSimEvent::pose:
            listener->onPose(myo, timestamp, simEvent.getPose());
            break;
        case myoSimEvent::rssi:
            listener->onRssi(myo, timestamp, simEvent.getRssi());
            break;
        default:
            break;
        }
    }
}

Myo* Hub::addMyo(unsigned int identifier)
{
    Myo* myo = new Myo(identifier);

    myos.push_back(myo);

    return myo;
}

unsigned short Hub::extractUnsignedShort(TCHAR* bytes)
{
    unsigned short us;
    memcpy(&us, bytes, sizeof(us));
    return us;
}

float Hub::extractFloat(TCHAR* bytes)
{
    float f;
    memcpy(&f, bytes, sizeof(f));
    return f;
}

void Hub::run(unsigned int duration_ms)
{
    // Run for duration_ms, read from named pipe for events, form a SimEvent
    // object based on received information, dispatch events to onDeviceEvent  
    std::vector<Myo*>::iterator it;
    DWORD startTime = GetTickCount();
    while (true)
    {
        if (GetTickCount() - startTime >= duration_ms) break;

        for (it = myos.begin(); it != myos.end(); it++)
        {
            DWORD actualBytes;

            if ((*it)->getReadTimeout() != duration_ms)
            {
                (*it)->setReadTimeout(duration_ms);
            }

            // Read 
            TCHAR header[PIPE_COMM_HEADER_SIZE];
            bool success = (*it)->readFromPipe(header, PIPE_COMM_HEADER_SIZE, &actualBytes);

            if (!success) continue;

            unsigned char dataSize = header[0];
            unsigned short eventType = extractUnsignedShort(&(header[1]));

            SimEvent evt;

            //TODO: Check that dataSize agrees with the expected sizes.
            if (eventType == ET_SYNC_DAT)
            {
                TCHAR dat[SYNC_DATA_NUM_BYTES];
                success = (*it)->readFromPipe(dat, SYNC_DATA_NUM_BYTES, &actualBytes);

                if (!success) continue;

                float floatData[SYNC_DATA_NUM_FLOATS];
                for (int i = 0; i < SYNC_DATA_NUM_FLOATS; i++)
                {
                    floatData[i] = extractFloat(&(dat[i * sizeof(float)]));
                }

                evt.setEventType(myoSimEvent::orientation);
                evt.setOrientation(floatData[0], floatData[1], floatData[2], floatData[3]);
                evt.setGyroscopeData(floatData[4], floatData[5], floatData[6]);
                evt.setAccelerometerData(floatData[7], floatData[8], floatData[9]);

            }
            else if (eventType == ET_POSE)
            {
                TCHAR dat[POSE_DATA_NUM_BYTES];
                success = (*it)->readFromPipe(dat, POSE_DATA_NUM_BYTES, &actualBytes);

                if (!success) continue;

                unsigned short poseType = extractUnsignedShort(dat);

                evt.setEventType(myoSimEvent::pose);
                evt.setPose(Pose((Pose::Type) poseType));
            }
            else if (eventType == ET_ARM_RECOGNIZED)
            {
                TCHAR dat[ARM_RECOGNIZED_DATA_NUM_BYTES];
                success = (*it)->readFromPipe(dat, ARM_RECOGNIZED_DATA_NUM_BYTES, &actualBytes);

                if (!success) continue;
                evt.setArm((Arm) dat[0]);
                evt.setXDirection((XDirection) dat[1]);
                evt.setEventType(myoSimEvent::armRecognized);
            }
            else
            {
                evt.setEventType(eventTypeToEventMap[(EventType) eventType]);
            }
                                            
            evt.setTimestamp(GetTickCount());
            evt.setMyoIdentifier((*it)->getIdentifier());

            onDeviceEvent(evt);        
        }
    }
}

void Hub::runOnce(unsigned int duration_ms)
{

}

Myo* Hub::lookupMyo(unsigned int myoIdentifier) const
{
    std::vector<Myo*>::const_iterator it;
    Myo* foundMyo = NULL;

    for (it = myos.begin(); it != myos.end(); it++)
    {
        if ((*it)->getIdentifier() == myoIdentifier)
        {
            foundMyo = *it;
            break;
        }
    }

    return foundMyo;
}