#include "stdafx.h"
#include "HubSim.hpp"
#include "DeviceListenerSim.h"
#include <string>
#include <iostream>


#define MAX_MESSAGE_LEN 47


namespace myoSim
{

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

                myo::Vector3<float> accelerometerData = myo::Vector3<float>(simEvent.getAccelerometerData(vectorIndex::first),
                    simEvent.getAccelerometerData(vectorIndex::second), simEvent.getAccelerometerData(vectorIndex::third));
                listener->onAccelerometerData(myo, timestamp, accelerometerData);

                myo::Vector3<float> gyroData = myo::Vector3<float>(simEvent.getGyroscopeData(vectorIndex::first),
                    simEvent.getGyroscopeData(vectorIndex::second), simEvent.getGyroscopeData(vectorIndex::third));
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
                // TODO: Implement some way of data packing, so that we don't
                // have to read the raw data here.
                TCHAR buffer[MAX_MESSAGE_LEN];
                unsigned int numBytes = MAX_MESSAGE_LEN;
                DWORD actualBytes;
                ZeroMemory(buffer, MAX_MESSAGE_LEN);

                bool success = (*it)->readFromPipe(buffer, numBytes, &actualBytes);

                if (success)
                {
					//get size of data
					int data_size = buffer[0];

					TCHAR data_type_temp[2];
					TCHAR data[43];

					//get the data/event type(2 bytes)

					for (int x = 0; x < 2; x++)
					{
						data_type_temp[x] = buffer[x + 1];
					}
					//get the data
					for (int y = 0; y < data_size; y++)
					{
						data[y] = buffer[y + 3];
					}
					
					std::string event_type(data_type_temp);

					
					SimEvent evt;
					//set event type
					//still need to add types for disconnect
					if (event_type == "onOrientationData")
					{
						evt.setEventType(myoSimEvent::orientation);
					}
					else if (event_type == "onPose")
					{
						evt.setEventType(myoSimEvent::pose);
						Pose pose();
						evt.setPose();
					}
					else if (event_type == "onAttach")
					{
						evt.setEventType(myoSimEvent::paired);
					}
					else if (event_type == "onConnect")
					{
						evt.setEventType(myoSimEvent::connected);
					}
					else if (event_type == "onArmSync")
					{
						evt.setEventType(myoSimEvent::armRecognized);
					}
					else
					{
						printf("data_type error\n");
						return;
					}


										
					//std::string message(buffer);
                   // SimEvent evt;

					
                    // TODO: For now, all data received is pose data. Must change to support all data. 
                    // Will do that once protocol has been decided.
                   // evt.setEventType(myoSimEvent::pose);

                    // TODO: Have a proper way of dealing with this. Do it in 
                    // a different class. Perhaps have the Myo class convert
                    // messages to events first.
                    /*if (message == "rest")
                    {
                        Pose pose(Pose::rest);
                        evt.setPose(pose);
                    }
                    else if (message == "fist")
                    {
                        Pose pose(Pose::fist);
                        evt.setPose(pose);
                    }
                    else if (message == "waveIn")
                    {
                        Pose pose(Pose::waveIn);
                        evt.setPose(pose);
                    }
                    else if (message == "waveOut")
                    {
                        Pose pose(Pose::waveOut);
                        evt.setPose(pose);
                    }
                    else if (message == "fingersSpread")
                    {
                        Pose pose(Pose::fingersSpread);
                        evt.setPose(pose);
                    }
                    else if (message == "thumbToPinky")
                    {
                        Pose pose(Pose::thumbToPinky);
                        evt.setPose(pose);
                    }
                    else
                    {
                        Pose pose(Pose::unknown);
                        evt.setPose(pose);
                    }*/

                    evt.setTimestamp(GetTickCount());
                    evt.setMyoIdentifier((*it)->getIdentifier());

                    onDeviceEvent(evt);
                }
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

}