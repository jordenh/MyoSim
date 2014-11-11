#include "HubSim.hpp"
#include "DeviceListenerSim.h"

using namespace myoSim;

HubSim::HubSim(const std::string& applicationIdentifier = "")
{
    // Do nothing
}

HubSim::~HubSim()
{
    // Do nothing
}

Myo* HubSim::waitForMyo(unsigned int milliseconds = 0)
{

}

void HubSim::addListener(DeviceListener* listener)
{
    if (std::find(listeners.begin(), listeners.end(), listener) != listeners.end()) {
        // Listener was already added.
        return;
    }

    listeners.push_back(listener);
}

void HubSim::removeListener(DeviceListener* listener)
{
    std::vector<DeviceListener*>::iterator it = std::find(listeners.begin(), listeners.end(), listener);

    if (it == listeners.end()) {
        // Listener is not in list.
        return;
    }

    listeners.erase(it);
}

void HubSim::onDeviceEvent(SimEvent simEvent)
{
    std::vector<DeviceListener*>::iterator it;
    uint64_t timestamp = simEvent.getEventTimestamp();

    Myo* myo = lookupMyo(simEvent.getMyoIdentifier);

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
            myo::Quaternion<float> orientation = myo::Quaternion<float>(simEvent.getXOrientation(), 
                simEvent.getYOrientation(),simEvent.getZOrientation(), simEvent.getWOrientation());
            listener->onOrientationData(myo, timestamp, orientation);

            myo::Vector3<float> accelerometerData = myo::Vector3<float>(simEvent.getAccelerometerData(vectorIndex::first),
                simEvent.getAccelerometerData(vectorIndex::second), simEvent.getAccelerometerData(vectorIndex::third));
            listener->onAccelerometerData(myo, timestamp, accelerometerData);

            myo::Vector3<float> gyroData = myo::Vector3<float>(simEvent.getGyroscopeData(vectorIndex::first),
                simEvent.getGyroscopeData(vectorIndex::second), simEvent.getGyroscopeData(vectorIndex::third));
            listener->onGyroscopeData(myo, timestamp, gyroData);
            break;
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

Myo* HubSim::addMyo(unsigned int identifier)
{
    Myo* myo = new Myo(identifier);

    myos.push_back(myo);

    return myo;
}

void HubSim::run(unsigned int duration_ms)
{

}

void HubSim::runOnce(unsigned int duration_ms)
{

}

Myo* HubSim::lookupMyo(unsigned int myoIdentifier) const
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