#include "stdafx.h"
#include "SimEvent.h"


SimEvent::SimEvent()
{
}


SimEvent::~SimEvent()
{
}

myoSimEvent SimEvent::getEventType() const
{
    return eventType;
}

uint64_t SimEvent::getEventTimestamp() const
{
    return eventTimestamp;
}

unsigned int SimEvent::getMyoIdentifier() const
{
    return myoIdentifier;
}

myoSim::Arm SimEvent::getArm() const
{
    return arm;
}

myoSim::XDirection SimEvent::getXDirection() const
{
    return xDirection;
}

float SimEvent::getXOrientation() const
{
    return xOrientation;
}

float SimEvent::getYOrientation() const
{
    return yOrientation;
}

float SimEvent::getZOrientation() const
{
    return zOrientation;
}

float SimEvent::getWOrientation() const
{
    return wOrientation;
}

float SimEvent::getAccelerometerData(vectorIndex index) const
{
    switch (index)
    {
    case vectorIndex::first:
        return accelerometerData[0];
        break;
    case vectorIndex::second:
        return accelerometerData[1];
        break;
    case vectorIndex::third:
        return accelerometerData[2];
        break;
    }
}

float SimEvent::getGyroscopeData(vectorIndex index) const
{
    switch (index)
    {
    case vectorIndex::first:
        return gyroscopeData[0];
        break;
    case vectorIndex::second:
        return gyroscopeData[1];
        break;
    case vectorIndex::third:
        return gyroscopeData[2];
        break;
    }
}

myo::Pose SimEvent::getPose() const
{
    return pose;
}

int8_t SimEvent::getRssi() const
{
    return rssi;
}