<template>
    <v-app>
        <v-app-bar app
                   color="primary"
                   hide-on-scroll
                   dark>
            <div class="d-flex align-center">
                Flight Sounds Simulator
            </div>
        </v-app-bar>

        <v-content>
            <FlightParameters />
        </v-content>
    </v-app>
</template>

<script>
    import FlightParameters from './components/FlightParameters';

    export default {
        name: 'App',

        components: {
            FlightParameters,
        },

        data: () => ({
            //
        }),
    };
</script>
