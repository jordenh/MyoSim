// Hub_console.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include "Hub_sim.hpp"
#include "myo/myo.hpp"



int _tmain(int argc, _TCHAR* argv[])
{
	try {
		printf("testing\n");
		myo::Hub_sim hub("com.example.Hub_console");
//		hub.runOnce(1000);
	} catch (const std::exception& e) {
		std::cerr << "Error: " << e.what() << std::endl;
		std::cerr << "Press enter to continue.";
		std::cin.ignore();
		return 1;
	}
	char temp_char;
	std::cin >> temp_char;
	return 0;
}
