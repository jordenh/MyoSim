#include "stdafx.h"
#include "SimEvent.h"

namespace myoSim
{

    SimEvent::SimEvent()
    {
    }


    SimEvent::~SimEvent()
    {
    }

    void SimEvent::setEventType(myoSimEvent type)
    {
        eventType = type;
    }

    void SimEvent::setTimestamp(uint64_t timestamp)
    {
        eventTimestamp = timestamp;
    }

    void SimEvent::setMyoIdentifier(int identifier)
    {
        myoIdentifier = identifier;
    }

    void SimEvent::setArm(myoSim::Arm arm)
    {
        this->arm = arm;
    }

    void SimEvent::setXDirection(myoSim::XDirection xDirection)
    {
        this->xDirection = xDirection;
    }

    void SimEvent::setOrientation(float xOrientation, float yOrientation,
        float zOrientation, float wOrientation)
    {
        this->xOrientation = xOrientation;
        this->yOrientation = yOrientation;
        this->zOrientation = zOrientation;
        this->wOrientation = wOrientation;
    }

    float SimEvent::getAccelerometerX() const
    {
        return accelX;
    }

    float SimEvent::getAccelerometerY() const
    {
        return accelY;
    }

    float SimEvent::getAccelerometerZ() const
    {
        return accelZ;
    }

    float SimEvent::getGyroscopeYawPerSecond() const
    {
        return gyroYaw;
    }

    float SimEvent::getGyroscopePitchPerSecond() const
    {
        return gyroPitch;
    }

    float SimEvent::getGyroscopeRollPerSecond() const
    {
        return gyroRoll;
    }

    void SimEvent::setAccelerometerData(float xAccel, float yAccel, float zAccel)
    {
        accelX = xAccel;
        accelY = yAccel;
        accelZ = zAccel;
    }

    void SimEvent::setGyroscopeData(float yawPerSecond, float pitchPerSecond, float rollPerSecond)
    {
        gyroYaw = yawPerSecond;
        gyroPitch = pitchPerSecond;
        gyroRoll = rollPerSecond;
    }

    void SimEvent::setPose(Pose pose)
    {
        this->pose = pose;
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

    Pose SimEvent::getPose() const
    {
        return pose;
    }

    int8_t SimEvent::getRssi() const
    {
        return rssi;
    }

}