#include "stdafx.h"
#include "MyoSim.h"
#include <iostream>

#define MYO_SIM_PIPE "\\\\.\\pipe\\BvrPipe"

namespace myoSim
{

    Myo::Myo(unsigned int id) : identifier(id), pipe(NULL)
    {
        pipeName = TEXT("\\\\.\\pipe\\BvrPipe");
    }

    Myo::~Myo()
    {
        if (pipe != NULL && pipe != INVALID_HANDLE_VALUE)
        {
            CloseHandle(pipe);
        }
    }

    void Myo::vibrate(VibrationType type)
    {
        std::cout << "Send a vibrate here!" << std::endl;
    }

    void Myo::requestRssi() const
    {
        std::cout << "Request RSSI here!" << std::endl;
    }

    bool Myo::connectToPipe(unsigned int timeout)
    {
        while (true)
        {
            pipe = CreateFile(pipeName, GENERIC_READ | GENERIC_WRITE, 0,
                NULL, OPEN_EXISTING, 0, NULL);

            if (pipe != INVALID_HANDLE_VALUE) break;

            if (GetLastError() != ERROR_PIPE_BUSY)
            {
                std::cout << "Could not open pipe. Last error=" << GetLastError() << std::endl;
                return false;
            }

            if (!WaitNamedPipe(pipeName, timeout))
            {
                printf("Could not open pipe: %3f second wait time out.", (float)timeout/1000);
                return false;
            }
        }

        return true;
    }

    void Myo::setReadTimeout(unsigned int timeout)
    {
        COMMTIMEOUTS timeouts;
        timeouts.ReadIntervalTimeout = 0;
        timeouts.ReadTotalTimeoutMultiplier = 0;
        timeouts.ReadTotalTimeoutConstant = timeout;
        timeouts.WriteTotalTimeoutConstant = timeouts.WriteTotalTimeoutMultiplier = 0;

        SetCommTimeouts(pipe, &timeouts);
    }

    DWORD Myo::getReadTimeout()
    {
        COMMTIMEOUTS timeouts;
        GetCommTimeouts(pipe, &timeouts);

        return timeouts.ReadTotalTimeoutConstant;
    }

    bool Myo::readFromPipe(TCHAR* buffer, unsigned int numBytes, DWORD* actualBytes)
    {
        //TODO: Come up with a protocol to read from the pipe (I.E. send the size
        // first, then the message, so packets can be distinguished). Then package the 
        // data up in a different way (currently very raw).
        BOOL success = ReadFile(pipe, buffer, numBytes*sizeof(TCHAR), actualBytes, NULL);

        if (!success)
        {
            std::cout << "ReadFile from pipe failed. Last error=" << GetLastError() << std::endl;
            return false;
        }

        return true;
    }

    unsigned int Myo::getIdentifier() const
    {
        return identifier;
    }

}