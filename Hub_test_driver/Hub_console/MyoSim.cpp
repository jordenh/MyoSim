#include "stdafx.h"
#include "MyoSim.h"

using namespace myoSim;

Myo::Myo(unsigned int id) : identifier(id)
{
}


Myo::~Myo()
{
}

unsigned int Myo::getIdentifier() const
{
    return identifier;
}