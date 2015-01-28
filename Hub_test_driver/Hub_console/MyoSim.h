#pragma once
#include <Windows.h>
#include <myo\cxx\Myo.hpp>

namespace myoSim {
    class Myo
    {
    public:
        // Pass-through enum to Myo Vibration Type.
        enum VibrationType {
            vibrationShort = libmyo_vibration_short,
            vibrationMedium = libmyo_vibration_medium,
            vibrationLong = libmyo_vibration_long
        };
        
        /* Basic constructors and destructors*/
        Myo(unsigned int id);
        ~Myo();

        /**
         *  Print out a statement saying the Myo was vibrated.
         *
         *  @param[in] type length of the vibration given by the enum
         *                  VibrationType.
         */
        void vibrate(VibrationType type);
        /**
         *  Print out a line saying the RSSI was requested from the Myo.
         */
        void requestRssi() const;

        /**
         *  Creates the pipe used by the simulated Myo and the hub to
         *  communicate with each other
         *
         *  @param[in]  timeout Number of milliseconds to wait for the pipe to
         *                      be created.
         *  @return     true    if pipe is successfully created
         *              false   otherwise
         */
        bool connectToPipe(unsigned int timeout);

        /**
         *  Read Myo data from the pipe.
         *
         *  @param[out] buffer  pointer to space that will hold the data read.
         *  @param[in]  numBytes    Maximum number of bytes to read.
         *  @param[out] actualBytes Number of bytes actually read.
         *  @return     true    if the pipe was successfully read from.
         *              false   otherwise.
         */
        bool readFromPipe(TCHAR* buffer, unsigned int numBytes, DWORD* actualBytes);

        /**
         *  Returns the identifier for the Myo sim.
         *
         *  @return Value stored in the private member identifier.
         */
        unsigned int getIdentifier() const;
        
        /**
         *  Set the timeout values for reading from the pipe.
         *
         *  @param[in]  timeout Variable containing the timeout in milliseconds
         *                      used for read operations through the pipe
         */
        void setReadTimeout(unsigned int timeout);

        /**
         *  Get the timeout value used when reading from the pipe.
         *
         *  @return A double containing the amount of timeout in milliseconds.
         */
        DWORD getReadTimeout();

    private:
        unsigned int identifier;
        HANDLE pipe;
        LPTSTR pipeName;
    };
}

