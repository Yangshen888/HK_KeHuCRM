import Vue from 'vue'
import App from './App'
import store from './store'
import checkusertoken from 'api/user/user.js'

import cuCustom from './colorui/components/cu-custom.vue'
Vue.component('cu-custom',cuCustom)

import gognneng from './pages/duoduoCRM/gognneng/gognneng.vue'
Vue.component('gognneng',gognneng)


import messages from './pages/duoduoCRM/message/messages.vue'
Vue.component('messages',messages)

import information from './pages/duoduoCRM/information/information.vue'
Vue.component('information',information)
import communList from './pages/duoduoCRM/communList/communList.vue'
Vue.component('communList',communList)


Vue.config.productionTip = false

Vue.prototype.$store = store
Vue.prototype.$checkusertoken = checkusertoken

App.mpType = 'app'

const app = new Vue({
	store,
    ...App
})
app.$mount()
