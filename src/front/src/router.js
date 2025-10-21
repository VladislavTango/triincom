import { createRouter, createWebHistory } from 'vue-router'
import LoanList from '../src/pages/LoandList.vue'
import AddLoan from '../src/pages/AddLoand.vue'

const routes = [
  { path: '/', name: 'LoanList', component: LoanList },
  { path: '/add', name: 'AddLoan', component: AddLoan },
]

export default createRouter({
  history: createWebHistory(),
  routes
})
