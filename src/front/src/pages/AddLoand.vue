<template>
  <a-card title="Добавить заявку" class="shadow-md rounded-2xl">
    <a-form layout="vertical" @submit.prevent="submitForm">
      <a-form-item label="Сумма" :validate-status="errors.amount ? 'error' : ''" :help="errors.amount">
        <a-input-number v-model:value="form.amount" :min="0" />
      </a-form-item>

      <a-form-item label="Срок займа (мес.)" :validate-status="errors.termValue ? 'error' : ''" :help="errors.termValue">
        <a-input-number v-model:value="form.termValue" :min="0" />
      </a-form-item>

      <a-form-item label="Процентная ставка (%)" :validate-status="errors.interestValue ? 'error' : ''" :help="errors.interestValue">
        <a-input-number v-model:value="form.interestValue" :min="0" />
      </a-form-item>

      <div class="flex justify-end">
        <a-button type="primary" html-type="submit" :loading="loading">Сохранить</a-button>
      </div>
    </a-form>
  </a-card>
</template>

<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { message } from 'ant-design-vue'
import {createLoan} from '../Http/LoanHttp'

const router = useRouter()
const loading = ref(false)

const form = reactive({
  amount: 0,
  termValue: 0,
  interestValue: 0
})

const errors = reactive({})

const validate = () => {
  let valid = true
  errors.amount = form.amount <= 0 ? 'Сумма должна быть больше 0' : ''
  errors.termValue = form.termValue <= 0 ? 'Срок займа должен быть больше 0' : ''
  errors.interestValue = form.interestValue <= 0 ? 'Процентная ставка должна быть больше 0' : ''
  valid = !Object.values(errors).some(e => e)
  return valid
}

const submitForm = async () => {
  if (!validate()) return
  
  loading.value = true
  
  try {
    await createLoan({
      amount: form.amount,
      termValue: form.termValue,
      interestValue: form.interestValue
    })
    
    message.success('Заявка успешно создана!')
    router.push('/')
    
  } catch (error) {
    message.error(error.response?.data?.response?.ErrorMessage)
  } finally {
    loading.value = false
  }
}
</script>