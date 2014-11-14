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
#include "HubSim.hpp"
#include "MyoSim.h"
#include "DeviceListenerSim.h"
#endif

#ifdef USE_SIMULATOR
using namespace myoSim;
#else
using namespace myo;
#endif

static bool isDisconnected = false;

// Classes that inherit from myo::DeviceListener can be used to receive events from Myo devices. DeviceListener
// provides several virtual functions for handling different kinds of events. If you do not override an event, the
// default behavior is to do nothing.
class DataCollector : public DeviceListener {
public:
    DataCollector()
        : currentPose()
    {
    }

    /// Called when a paired Myo has been disconnected.
    void onDisconnect(Myo* myo, uint64_t timestamp)
    {
        std::string temp;
        std::cout << "unknown received" << std::endl;
        isDisconnected = true;
        std::cin >> temp;
    }

    // onPose() is called whenever the Myo detects that the person wearing it has changed their pose, for example,
    // making a fist, or not making a fist anymore.
    void onPose(Myo* myo, uint64_t timestamp, myo::Pose pose)
    {
        if (currentPose.type() != pose.type())
        {
            std::string poseString = pose.toString();
            std::cout << "Current pose: " << poseString << std::endl;
        }

        currentPose = pose;
        // Vibrate the Myo whenever we've detected that the user has made a fist.
        if (pose == myo::Pose::fist) {
            myo->vibrate(myo::Myo::vibrationMedium);
        }
    }

    myo::Pose currentPose;
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
            if (isDisconnected)
            {
	            break;
            }
        }
    }
    catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << std::endl;
        std::cerr << "Press enter to continue.";
        std::cin.ignore();
        return 1;
    }
}

/*
#include "stdafx.h"
#include <Windows.h>
#include <myo/myo.hpp>

#define BUFFER_SIZE 512

int _tmain(int argc, _TCHAR* argv[])
{
    HANDLE pipe;
    LPTSTR pipeName = TEXT("\\\\.\\pipe\\BvrPipe");

    while (true)
    {
        pipe = CreateFile(pipeName, GENERIC_READ | GENERIC_WRITE, 0, 
            NULL, OPEN_EXISTING, 0, NULL);

        if (pipe != INVALID_HANDLE_VALUE) break;

        if (GetLastError() != ERROR_PIPE_BUSY)
        {
            std::cout << "Could not open pipe. Last error=" << GetLastError() << std::endl;
            return -1;
        }

        if (!WaitNamedPipe(pipeName, 20000))
        {
            printf("Could not open pipe: 20 second wait time out.");
            return -1;
        }
    }

    BOOL success;
    do
    {
        TCHAR buffer[BUFFER_SIZE];
        DWORD numBytes;
        ZeroMemory(buffer, BUFFER_SIZE);
        success = ReadFile(pipe, buffer, BUFFER_SIZE*sizeof(TCHAR), &numBytes, NULL);

        if (!success && GetLastError() != ERROR_MORE_DATA) break;

        std::cout << buffer << std::endl;
    } while (!success);

    if (!success)
    {
        std::cout << "ReadFile from pipe failed. Last error=" << GetLastError() << std::endl;
        return -1;
    }
    
    system("PAUSE");
    CloseHandle(pipe);
	return 0;
}

*/