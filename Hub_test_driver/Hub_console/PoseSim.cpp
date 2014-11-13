#include "stdafx.h"
#include "PoseSim.h"

namespace myoSim
{
    Pose::Pose()
    {
        simType = unknown;
        myo::Pose tmpPose = myo::Pose();
        myoPose = tmpPose;
    }

    Pose::Pose(Type type)
    {
        simType = type;
        myo::Pose::Type myoType = (myo::Pose::Type) ((int)type);
        myo::Pose tmpPose = myo::Pose(myoType);
        myoPose = tmpPose;
    }

    myo::Pose Pose::getUnderlyingPose() const
    {
        return myoPose;
    }

    /// Returns true if and only if the two poses are of the same type.
    bool Pose::operator==(Pose other) const
    {
        return myoPose == other.getUnderlyingPose();
    }

    /// Equivalent to `!(*this == other)`.
    bool Pose::operator!=(Pose other) const
    {
        return myoPose != other.getUnderlyingPose();
    }

    /// Returns the type of this pose.
    Pose::Type Pose::type() const
    {
        return simType;
    }

    /// Return a human-readable string representation of the pose.
    std::string Pose::toString() const
    {
        return myoPose.toString();
    }

    bool operator==(Pose pose, Pose::Type t)
    {
        return pose.type() == t;
    }

    bool operator==(Pose::Type type, Pose pose)
    {
        return pose == type;
    }

    bool operator!=(Pose pose, Pose::Type type)
    {
        return pose.type() != type;
    }

    bool operator!=(Pose::Type type, Pose pose)
    {
        return pose != type;
    }

    std::ostream& operator<<(std::ostream& out, const Pose& pose)
    {
        return out << pose.toString();
    }

}