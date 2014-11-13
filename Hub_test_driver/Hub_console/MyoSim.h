#pragma once
#include <Windows.h>
#include <myo\cxx\Myo.hpp>

namespace myoSim {
    class Myo
    {
    public:
        Myo(unsigned int id);
        ~Myo();

        void vibrate(myo::Myo::VibrationType type);
        void requestRssi() const;

        bool connectToPipe(unsigned int timeout);
        bool readFromPipe(TCHAR* buffer, unsigned int numBytes, DWORD* actualBytes);
        unsigned int getIdentifier() const;

    private:
        unsigned int identifier;
        HANDLE pipe;
        LPTSTR pipeName;
    };
}

