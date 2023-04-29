<template>
    <v-container>
        <v-row>
            <v-col>
                <p class="title">Current flight state: {{ current_flight_state }}</p>

                <v-container>
                    <v-row>
                        <v-col cols="12" sm="6" md="3" lg="3" xl="1">
                            <v-radio-group v-model="seat_belts" mandatory>
                                <p class="subtitle-1">Seat belts sign</p>
                                <v-radio label="Off" value="0"></v-radio>
                                <v-radio label="Auto" value="1"></v-radio>
                                <v-radio label="On" value="2"></v-radio>
                            </v-radio-group>
                        </v-col>

                        <v-col cols="12" sm="6" md="3" lg="3" xl="1">
                            <v-radio-group v-model="nose_lights" mandatory>
                                <p class="subtitle-1">Nose lights</p>
                                <v-radio label="Off" value="0"></v-radio>
                                <v-radio label="Taxi" value="1"></v-radio>
                                <v-radio label="Takeoff" value="2"></v-radio>
                            </v-radio-group>
                        </v-col>

                        <v-col cols="12" sm="6" md="3" lg="3" xl="1">
                            <v-radio-group v-model="landing_lights" mandatory>
                                <p class="subtitle-1">Landing lights</p>
                                <v-radio label="Off" value="0"></v-radio>
                                <v-radio label="Retracted" value="1"></v-radio>
                                <v-radio label="On" value="2"></v-radio>
                            </v-radio-group>
                        </v-col>

                        <v-col cols="12" sm="6" md="3" lg="3" xl="1">
                            <v-radio-group v-model="front_left_door" mandatory>
                                <p class="subtitle-1">Front left door</p>
                                <v-radio label="Closed" value="0"></v-radio>
                                <v-radio label="Opened" value="2"></v-radio>
                            </v-radio-group>
                        </v-col>

                        <v-col cols="12" sm="6" md="3" lg="3" xl="1">
                            <v-radio-group v-model="beacon_lights" mandatory>
                                <p class="subtitle-1">Beacon lights</p>
                                <v-radio label="Off" value="0"></v-radio>
                                <v-radio label="On" value="1"></v-radio>
                            </v-radio-group>
                        </v-col>

                        <v-col cols="12" sm="6" md="3" lg="3" xl="1">
                            <v-radio-group v-model="day_period" mandatory>
                                <p class="subtitle-1">Day period</p>
                                <v-radio label="Day" value="0"></v-radio>
                                <v-radio label="Night" value="1"></v-radio>
                            </v-radio-group>
                        </v-col>

                        <v-col cols="12" sm="6" md="3" lg="3" xl="1">
                            <v-radio-group v-model="cam_position" mandatory>
                                <p class="subtitle-1">Camera position</p>
                                <v-radio label="Cockpit" value="0"></v-radio>
                                <v-radio label="Cabin" value="1"></v-radio>
                                <v-radio label="External" value="2"></v-radio>
                            </v-radio-group>
                        </v-col>

                        <v-col cols="12" sm="6" md="3" lg="3" xl="1">
                            <v-radio-group v-model="is_paused" mandatory>
                                <p class="subtitle-1">State</p>
                                <v-radio label="Running" value="0"></v-radio>
                                <v-radio label="Paused" value="1"></v-radio>
                            </v-radio-group>
                        </v-col>

                        <v-col cols="12" sm="6" md="3" lg="3" xl="1">
                            <v-radio-group v-model="engines_state" mandatory>
                                <p class="subtitle-1">Engines State</p>
                                <v-radio label="Off" value="0"></v-radio>
                                <v-radio label="Running" value="1"></v-radio>
                            </v-radio-group>
                        </v-col>
                    </v-row>
                </v-container>

                <v-container>
                    <v-row>
                        <v-col cols="12" sm="12" md="4" lg="4" xl="4">
                            <p class="subtitle-1">Altitude above ground level ({{ alt_above_ground }}ft)</p>
                            <v-slider v-model="alt_above_ground" max="39000" step="500" />
                        </v-col>

                        <v-col cols="12" sm="12" md="4" lg="4" xl="4">
                            <p class="subtitle-1">Indicated vertical speed ({{ vertical_speed }}ft)</p>
                            <v-slider v-model="vertical_speed" min="-4000" max="4000" step="100" />
                        </v-col>

                        <v-col cols="12" sm="12" md="4" lg="4" xl="4">
                            <p class="subtitle-1">Ground speed ({{ ground_speed }}kts)</p>
                            <v-slider v-model="ground_speed" max="340" step="5"/>
                        </v-col>
                    </v-row>
                </v-container>
            </v-col>

            <v-container>
                <v-row>
                    <v-col class="text-sm-right text-center">
                        <v-btn color="primary" @click="postFlightParameters()">Set parameters</v-btn>
                    </v-col>
                </v-row>
            </v-container>
        </v-row>
    </v-container>
</template>

<script>
    import axios from "axios";

    export default {
        name: 'FlightParameters',

        data: () => ({
            current_flight_state: 'Parked',
            seat_belts: 0,
            nose_lights: 0,
            landing_lights: 0,
            beacon_lights: 0,
            front_left_door: 0,
            ground_speed: 0,
            alt_above_ground: 0,
            vertical_speed: 0,
            day_period: 0,
            cam_position: 0,
            is_paused: 0,
            engines_state: 0
        }),

        methods: {
            postFlightParameters: function () {
                axios.post("http://localhost:5000/flightparameters", {
                    seatBeltsSwitch: parseInt(this.seat_belts),
                    noseLightSwitch: parseInt(this.nose_lights),
                    landingLightsSwitch: parseInt(this.landing_lights),
                    beaconLightsSwtich: parseInt(this.beacon_lights),
                    frontLeftDoorState: parseInt(this.front_left_door),
                    dayPeriod: parseInt(this.day_period),
                    altitudeAboveGroundLevel: parseFloat(this.alt_above_ground),
                    indicatedVerticalSpeed: parseFloat(this.vertical_speed),
                    groundSpeed: parseFloat(this.ground_speed),
                    isInsideCockpit: parseInt(this.cam_position) == 0,
                    isCameraExternal: parseInt(this.cam_position) == 2,
                    isPaused: parseInt(this.is_paused) == 1,
                    enginesState: parseInt(this.engines_state)
                }).then(response => {
                    this.current_flight_state = response.data.currentFlightState;
                });
            }
        }
    }
</script>
