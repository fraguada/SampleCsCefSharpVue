import Vue from 'vue'
import Vuex from 'vuex'
import { EventBus } from '../eventbus'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
  },
  mutations: {
    ADD_TEXT (state, data) {
      console.log('store: about to emit')
      EventBus.$emit('SET_TEXT', data)
    }
  },
  actions: {
  },
  modules: {
  }
})
