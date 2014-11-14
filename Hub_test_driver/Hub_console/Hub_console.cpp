// Hub_console.cpp : Defines the entry point for the console application.
//
#include "stdafx.h"
#include <Windows.h>

// Copyright (C) 2013-2014 Thalmic Labs Inc.
// Distributed under the Myo SDK license agreement. See LICENSE.txt for details.
#define _USE_MATH_DEFINES
#include <cmath>
#include <iostream>
#include <iomanip>
#include <stdexcept>
#include <string>
#include <algorithm>
// The only file that needs to be included to use the Myo C++ SDK is myo.hpp.
#include <myo/myo.hpp>

#define USE_SIMULATOR

#ifdef USE_SIMULATOR
#include "MyoSimIncludes.hpp"
#endif

#ifdef USE_SIMULATOR
using namespace myoSim;
#else
using namespace myo;
#endif

// Classes that inherit from myo::DeviceListener can be used to receive events from Myo devices. DeviceListener
// provides several virtual functions for handling different kinds of events. If you do not override an event, the
// default behavior is to do nothing.
class DataCollector : public DeviceListener {
public:
    DataCollector()
        : currentPose()
    {
    }

    // onPose() is called whenever the Myo detects that the person wearing it has changed their pose, for example,
    // making a fist, or not making a fist anymore.
    void onPose(Myo* myo, uint64_t timestamp, Pose pose)
    {
        if (currentPose.type() != pose.type())
        {
            std::string poseString = pose.toString();
            std::cout << "Current pose: " << poseString << std::endl;
        }

        currentPose = pose;
        // Vibrate the Myo whenever we've detected that the user has made a fist.
        if (pose == Pose::fist) {
            myo->vibrate(Myo::vibrationMedium);
        }
    }

    Pose currentPose;
};

int main(int argc, char** argv)
{
    try {
        Hub hub("com.example.hello-myo");
        std::cout << "Attempting to find a Myo..." << std::endl;

        Myo* myo = hub.waitForMyo(10000);

        if (!myo) {
            throw std::runtime_error("Unable to find a Myo!");
        }

        std::cout << "Connected to a Myo armband!" << std::endl << std::endl;

        DataCollector collector;
        hub.addListener(&collector);

        while (1) {
            hub.run(1000 / 20);
        }
    }
    catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << std::endl;
        std::cerr << "Press enter to continue.";
        std::cin.ignore();
        return 1;
    }
}