#pragma once
#include <cstdint>
#include "DeviceListenerSim.h"

#define ACCELEROMETER_MAX_INDEX 3
#define GYROSCOPE_MAX_INDEX 3

enum class myoSimEvent {
    paired,
    unpaired,
    connected,
    disconnected,
    armRecognized,
    armLost,
    orientation,
    pose,
    rssi
};

enum class vectorIndex {
    first,
    second,
    third
};

class SimEvent
{
public:
    SimEvent();
    ~SimEvent();

    void setEventType(myoSimEvent type);
    void setTimestamp(uint64_t timestamp);
    void setMyoIdentifier(int identifier);
    void setArm(myoSim::Arm arm);
    void setXDirection(myoSim::XDirection xDirection);
    void setOrientation(float xOrientation, float yOrientation, 
        float zOrientation, float wOrientation);
    void setAccelerometerData(vectorIndex index, float data);
    void setGyroscopeData(vectorIndex index, float data);
    void setPose(myo::Pose pose);

    myoSimEvent getEventType() const;
    uint64_t getEventTimestamp() const;
    unsigned int getMyoIdentifier() const;
    myoSim::Arm getArm() const;
    myoSim::XDirection getXDirection() const;

    float getXOrientation() const;
    float getYOrientation() const;
    float getZOrientation() const;
    float getWOrientation() const;

    float getAccelerometerData(vectorIndex index) const;
    float getGyroscopeData(vectorIndex index) const;

    myo::Pose getPose() const;
    int8_t getRssi() const;

private:
    myoSimEvent eventType;
    uint64_t eventTimestamp;
    unsigned int myoIdentifier;
    myoSim::Arm arm;
    myoSim::XDirection xDirection;
    float xOrientation, yOrientation, zOrientation, wOrientation;
    float accelerometerData[ACCELEROMETER_MAX_INDEX];
    float gyroscopeData[GYROSCOPE_MAX_INDEX];
    myo::Pose pose;
    int8_t rssi;
};
