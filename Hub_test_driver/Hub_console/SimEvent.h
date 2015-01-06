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
        /**
         *  Set the pose for this event
         *
         *  @param[in]  pose    A pose from the Pose class
         */
        void setPose(Pose pose);

        /**
         *  Get the type of this event
         *
         *  @return The event type of this event found in the enum myoSimEvent
         */
        myoSimEvent getEventType() const;
        /**
         *  Get the timestamp of the event
         *
         *  @return The timestamp of the event in milliseconds
         */
        uint64_t getEventTimestamp() const;
        /**
         *  Get the identifier number of the Myo
         *
         *  @return The identifier for the Myo device
         */
        unsigned int getMyoIdentifier() const;
        /**
         *  Get the arm the Myo is currently on
         *
         *  @return The value corresponding to the arm the Myo is on
         */
        Arm getArm() const;
        /**
         *  Get the xDirection orientation of the Myo
         *
         *  @return The orientation of the Myo on the arm
         */
        XDirection getXDirection() const;

        /**
         *  Get the X coordinate of the quaternion
         *
         *  @return A float corresponding the X coordinate value
         */
        float getXOrientation() const;
        /**
         *  Get the Y coordinate of the quaternion
         *
         *  @return A float corresponding the Y coordinate value
         */
        float getYOrientation() const;
        /**
         *  Get the Z coordinate of the quaternion
         *
         *  @return A float corresponding the Z coordinate value
         */
        float getZOrientation() const;
        /**
         *  Get the angle of the quaternion
         *
         *  @return A float corresponding the angle
         */
        float getWOrientation() const;

        /**
         *  Get the X coordinate of the acceleration
         *
         *  @return A float cooresponding to the X coordinate of the
         *          acceleration
         */
        float getAccelerometerX() const;
        /**
         *  Get the X coordinate of the acceleration
         *
         *  @return A float cooresponding to the Y coordinate of the
         *          acceleration
         */
        float getAccelerometerY() const;
        /**
         *  Get the Z coordinate of the acceleration
         *
         *  @return A float cooresponding to the Z coordinate of the
         *          acceleration
         */
        float getAccelerometerZ() const;

        /**
         *  Get the yaw portion of the gyroscope data
         *
         *  @return A float corresponding to the yaw of the gyroscope
         */
        float getGyroscopeYawPerSecond() const;
        /**
         *  Get the pitch portion of the gyroscope data
         *
         *  @return A float corresponding to the pitch of the gyroscope
         */
        float getGyroscopePitchPerSecond() const;
        /**
         *  Get the roll portion of the gyroscope data
         *
         *  @return A float corresponding to the roll of the gyroscope
         */
        float getGyroscopeRollPerSecond() const;

        /**
         *  Get the pose of this event
         *
         *  @return A pose from the class Pose
         */
        Pose getPose() const;
        /**
         *  Get the RSSI of the Myo
         *
         *  @return A number corresponding to the RSSI
         */
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