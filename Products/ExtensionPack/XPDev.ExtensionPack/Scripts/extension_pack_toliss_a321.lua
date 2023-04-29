METERS_TO_FEET = 3.28084
METERS_PER_SECOND_TO_KNOTS = 1.94384

indicatedAltitudeFt = create_dataref("xpdev/data/indicatedAltitudeFt", "number")
indicatedAltitudeAboveGroundFt = create_dataref("xpdev/data/indicatedAltitudeAboveGroundFt", "number")
indicatedVerticalSpeedFpm = create_dataref("xpdev/data/indicatedVerticalSpeedFpm", "number")
indicatedGroundSpeed = create_dataref("xpdev/data/indicatedGroundSpeed", "number")
fmcFlightPhase = create_dataref("xpdev/data/fmcFlightPhase", "number")
enginesN2RatePercent = create_dataref("xpdev/data/enginesN2RatePercent", "number")
flapsLeverPosition = create_dataref("xpdev/data/flapsLeverPosition", "number")
gearLeverPosition = create_dataref("xpdev/data/gearLeverSwitchPosition", "number")
paxDoorsStates = create_dataref("xpdev/data/paxDoorsStates", "array[4]")
seatBeltsSwtichPosition = create_dataref("xpdev/data/seatBeltsSwitchPosition", "number")
taxiLightSwitchPosition = create_dataref("xpdev/data/taxiLightSwitchPosition", "number")
noseLightSwitchPosition = create_dataref("xpdev/data/noseLightSwitchPosition", "number")
beaconLightSwitchPosition = create_dataref("xpdev/data/beaconLightSwitchPosition", "number")
landingLightsSwitchPosition = create_dataref("xpdev/data/landingLightsSwitchPosition", "number")
cruiseLevelFt = create_dataref("xpdev/data/cruiseLevelFt", "number")
cockpitDoorPosition = create_dataref("xpdev/data/cockpitDoorPosition", "array[3]")
cockpitDoorState = create_dataref("xpdev/data/cockpitDoorState", "number")
cameraPosition = create_dataref("xpdev/data/cameraPosition", "array[3]")
externalView = create_dataref("xpdev/data/externalView", "number")
dayPeriod = create_dataref("xpdev/data/dayPeriod", "number")

camera_x = find_dataref("sim/graphics/view/pilots_head_x")
camera_y = find_dataref("sim/graphics/view/pilots_head_y")
camera_z = find_dataref("sim/graphics/view/pilots_head_z")
sun_pitch_degrees = find_dataref("sim/graphics/scenery/sun_pitch_degrees")

altitude_ft_pilot = find_dataref("sim/cockpit2/gauges/indicators/altitude_ft_pilot")
y_agl = find_dataref("sim/flightmodel/position/y_agl")
vvi_fpm_pilot = find_dataref("sim/cockpit2/gauges/indicators/vvi_fpm_pilot")
ground_speed = find_dataref("sim/flightmodel/position/groundspeed")
engn_n2 = find_dataref("sim/flightmodel/engine/ENGN_N2_[0]")
flaprqst = find_dataref("sim/flightmodel/controls/flaprqst")
gear_handle_down = find_dataref("sim/cockpit2/controls/gear_handle_down")
pax_door_array = find_dataref("AirbusFBW/PaxDoorArray")
seat_belt_signs_on = find_dataref("AirbusFBW/SeatBeltSignsOn")
nose_light = find_dataref("AirbusFBW/OHPLightSwitches[3]")
beacon_light = find_dataref("AirbusFBW/OHPLightSwitches[0]")
landing_light_L = find_dataref("AirbusFBW/OHPLightSwitches[4]")
landing_light_R = find_dataref("AirbusFBW/OHPLightSwitches[5]")
cruise_alt = find_dataref("toliss_airbus/init/cruise_alt")
flight_phase = find_dataref("AirbusFBW/APPhase")
ckpt_door = find_dataref("ckpt/door")
view_is_external = find_dataref("sim/graphics/view/view_is_external")

function flight_start()
	cockpitDoorPosition[0] = 0.481576
	cockpitDoorPosition[1] = 1.673985
	cockpitDoorPosition[2] = -16.706123
end

function after_physics()
	indicatedAltitudeFt = altitude_ft_pilot
	indicatedAltitudeAboveGroundFt = y_agl * METERS_TO_FEET
	indicatedVerticalSpeedFpm = vvi_fpm_pilot
	indicatedGroundSpeed = ground_speed	* METERS_PER_SECOND_TO_KNOTS
	enginesN2RatePercent = engn_n2
	flapsLeverPosition = flaprqst
	gearLeverPosition = gear_handle_down
	taxiLightSwitchPosition = nose_light % 1
	beaconLightSwitchPosition = beacon_light
	cruiseLevelFt = cruise_alt
	fmcFlightPhase = flight_phase
	cockpitDoorState = ckpt_door
	externalView = view_is_external
	
	if seat_belt_signs_on == 1 then
		seatBeltsSwtichPosition = 2
	else
		seatBeltsSwtichPosition = 0
	end
	
	if landing_light_L == 2 and landing_light_R == 2 then
		landingLightsSwitchPosition = 2
	elseif landing_light_L == 1 and landing_light_R == 1 then
		landingLightsSwitchPosition = 1
	else
		landingLightsSwitchPosition = 0
	end
	
	if nose_light == 1 then
		noseLightSwitchPosition = 0
		taxiLightSwitchPosition = 1
	elseif nose_light > 1 then
		noseLightSwitchPosition = 1
		taxiLightSwitchPosition = 1
	else
		noseLightSwitchPosition = 0
		taxiLightSwitchPosition = 0
	end

	paxDoorsStates[0] = pax_door_array[0]
	paxDoorsStates[1] = pax_door_array[6]
	
	if sun_pitch_degrees >= -6.0 then
		dayPeriod = 0
	else
		dayPeriod = 1
	end

	cameraPosition[0] = camera_x
	cameraPosition[1] = camera_y
	cameraPosition[2] = camera_z
end