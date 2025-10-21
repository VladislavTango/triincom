<template>
  <a-card title="Список заявок">
    <div >
      <a-button type="primary" @click="$router.push('/add')">Добавить заявку</a-button>
    </div>

<div>
      <a-select 
        v-model:value="filterStatus" 
        placeholder="Статус" 
        style="width: 180px" 
        @change="handleFilterChange"
      >
        <a-select-option value="">Все</a-select-option>
        <a-select-option value="1">Опубликованные</a-select-option>
        <a-select-option value="0">Снятые с публикации</a-select-option>
      </a-select>
      <a-input-number 
        v-model:value="filterMinAmount" 
        placeholder="Мин. сумма" 
        style="width: 200px;"
        @change="handleFilterChange" 
      />
      <a-input-number 
        v-model:value="filterMaxAmount" 
        placeholder="Макс. сумма" 
        style="width: 200px;"
        @change="handleFilterChange" 
      />
      <a-input-number 
        v-model:value="filterTermMin" 
        style="width: 200px;"
        placeholder="Срок займа от" 
        @change="handleFilterChange" 
      />
            <a-input-number 
        v-model:value="filterTermMax" 
        style="width: 200px;"
        placeholder="Срок займа до" 
        @change="handleFilterChange" 
      />
    </div>


  <a-table
    :columns="columns"
    :data-source="loans"
    :pagination="false"
    row-key="id"
    bordered
    :loading="loading"
  >
      <template #status="{ record }">
        <a-tag :color="record.status === 1 ? 'green' : 'red'">
          {{ record.status === 1 ? 'Опубликована' : 'Снята с публикации' }}
        </a-tag>
      </template>
      <template #actions="{ record }">
        <a-button
          type="link"
          @click="toggleStatus(record)"
        >
          {{ record.status === 1 ? 'Снять с публикации' : 'Опубликовать' }}
        </a-button>
      </template>
    </a-table>

    <div >
      <a-pagination
        :current="pageNumber"
        :total="totalCount"
        :page-size="pageSize"
        @change="onPageChange"
        show-size-changer
        @showSizeChange="onPageSizeChange"
      />
    </div>
  </a-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { message } from 'ant-design-vue'
import {getAllLoans, updateLoanStatus} from '../Http/LoanHttp'

const loans = ref([])
const loading = ref(false)
const pageNumber = ref(1)
const pageSize = ref(10)
const totalCount = ref(0)
const filterStatus = ref('')
const filterMinAmount = ref(null)
const filterMaxAmount = ref(null)
const filterTermMin = ref(null)
const filterTermMax = ref(null)

const columns = [
  { title: 'Номер заявки', dataIndex: 'number', key: 'number' },
  { title: 'Сумма', dataIndex: 'amount', key: 'amount' },
  { title: 'Срок займа', dataIndex: 'termValue', key: 'termValue' },
  { title: 'Процентная ставка', dataIndex: 'interestValue', key: 'interestValue' },
  { title: 'Статус', key: 'status', slots: { customRender: 'status' } },
  { 
    title: 'Дата создания', 
    dataIndex: 'createdAt', 
    key: 'createdAt',
    customRender: ({ text }) => new Date(text).toLocaleString('ru-RU')
  },
  { title: 'Действия', key: 'actions', slots: { customRender: 'actions' } },
]

const handleFilterChange = () => {
  pageNumber.value = 1 
  loadLoans()
}

const loadLoans = async () => {
  loading.value = true
  try {
    const params = {
      PageNumber: pageNumber.value,
      PageSize: pageSize.value,
      Status: filterStatus.value || null,
      MinAmount: filterMinAmount.value || null,
      MaxAmount: filterMaxAmount.value || null,
      Minterm: filterTermMin.value || null,
      MaxTerm: filterTermMax.value || null
    }

    const response = await getAllLoans(params)
    loans.value = response.data.response.data
    totalCount.value = response.data.response.totalCount
  } catch (error) {
    message.error(error.response.data.response.ErrorMessage || 'Ошибка загрузки данных')
  } finally {
    loading.value = false
  }
}



const toggleStatus = async (loan) => {
  const newStatus = loan.status === 1 ? 0 : 1
  const originalStatus = loan.status
  
  try {
    loan.status = newStatus
    
    await updateLoanStatus({
      status: newStatus,
      number: loan.number
    })
  } catch (error) {
    loan.status = originalStatus
    message.error(error.response?.data?.response?.ErrorMessage || 'Ошибка обновления статуса')
  }
}

const onPageChange = (page) => {
  pageNumber.value = page
  loadLoans()
}

const onPageSizeChange = (current, size) => {
  pageSize.value = size
  loadLoans()
}

onMounted(loadLoans)
</script>

<style scoped>
</style>
