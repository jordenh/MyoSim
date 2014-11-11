#pragma once

namespace myoSim {
    class Myo
    {
    public:
        Myo(unsigned int id);
        ~Myo();

        unsigned int getIdentifier() const;

    private:
        unsigned int identifier;
    };
}

