/*
 * Hub_sim_impl.hpp
 * 
 * Implementation for the Hub simultor methods.
 */
#include "../Hub_sim.hpp"

#include <algorithm>
#include <exception>
#include <Windows.h>

#include "myo/cxx/DeviceListener.hpp"
#include "myo/cxx/Myo.hpp"
#include "myo/cxx/Pose.hpp"
#include "myo/cxx/Quaternion.hpp"
#include "myo/cxx/Vector3.hpp"
#include "myo/cxx/detail/ThrowOnError.hpp"

namespace myo {
	void test_fnc(void)
	{
		printf("all the testing");
	}

	inline
		Hub_sim::Hub_sim(const std::string& applicationIdentifier)
		: _hub(0)
		, _myos()
		, _listeners()
	{
		/// Create connection here
		//libmyo_init_hub(&_hub, applicationIdentifier.c_str(), ThrowOnError());
	}

	inline
		Hub_sim::~Hub_sim()
	{
		for (std::vector<Myo*>::iterator I = _myos.begin(), IE = _myos.end(); I != IE; ++I) {
			delete *I;
		}
		libmyo_shutdown_hub(_hub, 0);
	}

	inline
		Myo* Hub_sim::waitForMyo(unsigned int timeout_ms)
	{
		std::size_t prevSize = _myos.size();

		struct local {
			static libmyo_handler_result_t handler(void* user_data, libmyo_event_t event) {
				Hub_sim* hub_sim = static_cast<Hub_sim*>(user_data);

				/// replace this with get from socket call
				libmyo_myo_t opaque_myo = libmyo_event_get_myo(event);

				switch (libmyo_event_get_type(event)) {
				case libmyo_event_paired:
					hub_sim->addMyo(opaque_myo);
					return libmyo_handler_stop;
				default:
					break;
				}

				return libmyo_handler_continue;
			}
		};

		do {
			hub_run(_hub, timeout_ms ? timeout_ms : 1000, &local::handler, this, ThrowOnError());
		} while (!timeout_ms && _myos.size() <= prevSize);

		if (_myos.size() <= prevSize) {
			return 0;
		}

		return _myos.back();
	}

	inline
		void Hub_sim::addListener(DeviceListener* listener)
	{
		if (std::find(_listeners.begin(), _listeners.end(), listener) != _listeners.end()) {
			// Listener was already added.
			return;
		}
		_listeners.push_back(listener);
	}

	inline
		void Hub_sim::removeListener(DeviceListener* listener)
	{
		std::vector<DeviceListener*>::iterator I = std::find(_listeners.begin(), _listeners.end(), listener);
		if (I == _listeners.end()) {
			// Don't have this listener.
			return;
		}

		_listeners.erase(I);
	}

	inline
		void Hub_sim::onDeviceEvent(libmyo_event_t event)
	{
		/* allow only 1 sim myo */
		libmyo_myo_t opaqueMyo = libmyo_event_get_myo(event);

		Myo* myo = lookupMyo(opaqueMyo);

		if (!myo && libmyo_event_get_type(event) == libmyo_event_paired) {
			myo = addMyo(opaqueMyo);
		}
		
		if (!myo) {
			// Ignore events for Myos we don't know about.
			return;
		}

		for (std::vector<DeviceListener*>::iterator I = _listeners.begin(), IE = _listeners.end(); I != IE; ++I) {
			DeviceListener* listener = *I;

			listener->onOpaqueEvent(event);

			uint64_t time = libmyo_event_get_timestamp(event);

			switch (libmyo_event_get_type(event)) {
			case libmyo_event_paired: {
				/* Fake firmware number for sim*/
				FirmwareVersion version = { 1, 0, 0, 0 };
				listener->onPair(myo, time, version);
				break;
			}
			case libmyo_event_unpaired:
				listener->onUnpair(myo, time);
				break;
			case libmyo_event_connected: {
				FirmwareVersion version = { libmyo_event_get_firmware_version(event, libmyo_version_major),
					libmyo_event_get_firmware_version(event, libmyo_version_minor),
					libmyo_event_get_firmware_version(event, libmyo_version_patch),
					libmyo_event_get_firmware_version(event, libmyo_version_hardware_rev) };
				listener->onConnect(myo, time, version);
				break;
			}
			case libmyo_event_disconnected:
				listener->onDisconnect(myo, time);
				break;
			case libmyo_event_arm_recognized:
				listener->onArmRecognized(myo, time,
					static_cast<Arm>(libmyo_event_get_arm(event)),
					static_cast<XDirection>(libmyo_event_get_x_direction(event)));
				break;
			case libmyo_event_arm_lost:
				listener->onArmLost(myo, time);
				break;
			case libmyo_event_orientation:
				listener->onOrientationData(myo, time,
					Quaternion<float>(libmyo_event_get_orientation(event, libmyo_orientation_x),
					libmyo_event_get_orientation(event, libmyo_orientation_y),
					libmyo_event_get_orientation(event, libmyo_orientation_z),
					libmyo_event_get_orientation(event, libmyo_orientation_w)));
				listener->onAccelerometerData(myo, time,
					Vector3<float>(libmyo_event_get_accelerometer(event, 0),
					libmyo_event_get_accelerometer(event, 1),
					libmyo_event_get_accelerometer(event, 2)));

				listener->onGyroscopeData(myo, time,
					Vector3<float>(libmyo_event_get_gyroscope(event, 0),
					libmyo_event_get_gyroscope(event, 1),
					libmyo_event_get_gyroscope(event, 2)));

				break;
			case libmyo_event_pose:
				listener->onPose(myo, time, Pose(static_cast<Pose::Type>(libmyo_event_get_pose(event))));
				break;
			case libmyo_event_rssi:
				listener->onRssi(myo, time, libmyo_event_get_rssi(event));
				break;
			}
		}
	}

	inline
		void Hub_sim::run(unsigned int duration_ms)
	{
		struct local {
			static libmyo_handler_result_t handler(void* user_data, libmyo_event_t event) {
				Hub_sim* hub_sim = static_cast<Hub_sim*>(user_data);

				hub_sim->onDeviceEvent(event);

				return libmyo_handler_continue;
			}
		};
		libmyo_run(_hub, duration_ms, &local::handler, this, ThrowOnError());
	}

	inline
		void Hub_sim::runOnce(unsigned int duration_ms)
	{
		struct local {
			static libmyo_handler_result_t handler(void* user_data, libmyo_event_t event) {
				Hub_sim* hub_sim = static_cast<Hub_sim*>(user_data);

				hub_sim->onDeviceEvent(event);

				return libmyo_handler_stop;
			}
		};
		libmyo_run(_hub, duration_ms, &local::handler, this, ThrowOnError());
	}

	inline
		libmyo_hub_t Hub_sim::libmyoObject()
	{
		return _hub;
	}

	inline
		Myo* Hub_sim::lookupMyo(libmyo_myo_t opaqueMyo) const
	{
		Myo* myo = 0;
		for (std::vector<Myo*>::const_iterator I = _myos.begin(), IE = _myos.end(); I != IE; ++I) {
			if ((*I)->libmyoObject() == opaqueMyo) {
				myo = *I;
				break;
			}
		}

		return myo;
	}

	inline
		Myo* Hub_sim::addMyo(libmyo_myo_t opaqueMyo)
	{
		Myo* myo = new Myo(opaqueMyo);

		_myos.push_back(myo);

		return myo;
	}

	/***** Private methods *****/

	libmyo_result_t Hub_sim::hub_run(libmyo_hub_t hub, unsigned int duration_ms, libmyo_handler_t handler, void* user_data,
		libmyo_error_details_t* out_error)
	{
		ULONGLONG start_time;
		ULONGLONG end_time;
		libmyo_handler_result_t ret;
		
		start_time = GetTickCount64();

		do {
			ret = handler(this, ThrowOnError());
			end_time = GetTickCount64();
		} while ((end_time - start_time) >= duration_ms);
		return libmyo_success;
	}

} // namespace myo
