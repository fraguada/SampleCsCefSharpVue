<template>
  <v-container>
    <v-btn @click.native='doSomethingRhino'>Do something in Rhino</v-btn>
  </v-container>
</template>

<script>
import { EventBus } from '../eventbus'
export default {
  data: () => ({
  }),
  methods: {
    doSomethingRhino () {
      const args = { location: [0, 0, 0], radius: 20 }
      // eslint-disable-next-line
        Interop.doSomething(args)
    }
  },
  mounted () {
    EventBus.$on('SET_TEXT', (data) => {
      // eslint-disable-next-line
      console.log('HelloWorld: ' + data)
    })
  }
}
</script>
