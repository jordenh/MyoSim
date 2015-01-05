#pragma once
#include <cstdint>
#include "DeviceListenerSim.h"

#define ACCELEROMETER_MAX_INDEX 3
#define GYROSCOPE_MAX_INDEX 3

namespace myoSim
{
    /**
     *  Enum for the different events available.
     *
     *  \relates libmyo_event_type_t
     */
    enum class myoSimEvent {
        paired,         /**< Myo is paired */
        unpaired,       /**< Myo is unpaired */
        connected,      /**< Myo is connected */
        disconnected,   /**< Myo is disconnected */
        armRecognized,  /**< Myo is detected on arm */
        armLost,        /**< Myo lost detection of arm */
        orientation,    /**< Sending orientation data */
        pose,           /**< Pose data requested, send pose data */
        rssi            /**< RSSI requested, send RSSI */
    };

    /**
     *  Enum used for the index of the three vector used in the acceleration
     *  and gyroscope data.
     */
    enum class vectorIndex {
        first,      /**< First value of 3 vector */
        second,     /**< Second value of 3 vector */
        third       /**< Third value of 3 vector */
    };

    /**
     *  SimEvent is used to keep track of the simulated Myo data. Things like
     *  events, orientation, acceleration, and gyroscope data for the simulated
     *  Myo is stored here as an event before it is passed to the hub.
     */
    class SimEvent
    {
    public:
        SimEvent();
        ~SimEvent();

        /**
         *  Set the event type of this simulated event.
         *
         *  @param[in]  type    event type of this event
         */
        void setEventType(myoSimEvent type);
        /**
         *  Set the timestamp of the event
         *
         *  @param[in]  timestamp   timestamp to set
         */
        void setTimestamp(uint64_t timestamp);
        /**
         *  Set the identifier to the simulated Myo
         *
         *  @param[in]  identifier  identifier number for the Myo
         */
        void setMyoIdentifier(int identifier);
        /**
         *  Set the arm the Myo is on
         *
         *  @param[in] arm left, right, or unknown value found in the enum Arm
         */
        void setArm(Arm arm);
        /**
         *  Set the xDirection for the Myo. The xDirection is the direction the
         *  Myo points. Either towards the wrist or the elbow, given by the
         *  enum xDirection.
         *
         *  @param[in] xDirection   Orientation of the Myo
         */
        void setXDirection(XDirection xDirection);
        /**
         *  Set the orientation data of this event packet
         *
         *  @param[in]  xOrientation X component of the quaternion
         *  @param[in]  yOrientation Y component of the quaternion
         *  @param[in]  xOrientation Z component of the quaternion
         *  @param[in]  wOrientation Angle of the quaternion
         */
        void setOrientation(float xOrientation, float yOrientation,
            float zOrientation, float wOrientation);
        /**
         *  Set the accelerometer data for this event
         *
         *  @param[in] xAccel   X component of the acceleration
         *  @param[in] yAccel   Y compnenet of the acceleration
         *  @param[in] zAccel   Z component of the acceleration
         */
        void setAccelerometerData(float xAccel, float yAccel, float zAccel);
        /**
         *  Set the gyroscopic data for this event
         *
         *  @param[in] yawPerSecond
         *  @param[in] pitchPerSecond
         *  @param[in] rollPerSecond
         */
        void setGyroscopeData(float yawPerSecond, float pitchPerSecond, float rollPerSecond);
        void setPose(Pose pose);

        myoSimEvent getEventType() const;
        uint64_t getEventTimestamp() const;
        unsigned int getMyoIdentifier() const;
        Arm getArm() const;
        XDirection getXDirection() const;

        float getXOrientation() const;
        float getYOrientation() const;
        float getZOrientation() const;
        float getWOrientation() const;

        float getAccelerometerX() const;
        float getAccelerometerY() const;
        float getAccelerometerZ() const;

        float getGyroscopeYawPerSecond() const;
        float getGyroscopePitchPerSecond() const;
        float getGyroscopeRollPerSecond() const;

        Pose getPose() const;
        int8_t getRssi() const;

    private:
        myoSimEvent eventType;
        uint64_t eventTimestamp;
        unsigned int myoIdentifier;
        Arm arm;
        XDirection xDirection;
        float xOrientation, yOrientation, zOrientation, wOrientation;
        float accelX, accelY, accelZ;
        float gyroYaw, gyroPitch, gyroRoll;
        Pose pose;
        int8_t rssi;
    };

}