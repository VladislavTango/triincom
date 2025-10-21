import axios from 'axios'

export const getAllLoans = (params) => {
  return axios.get('http://localhost:7225/api/AllFiltered', { params })
}

export const createLoan = (loanData) => {
  return axios.post('http://localhost:7225/api/loans', loanData, {
    headers: {
      'accept': '*/*',
      'Content-Type': 'application/json'
    }
  })
}

export const updateLoanStatus = (loanData) => {
  return axios.put('http://localhost:7225/api/', loanData, {
    headers: {
      'Accept': '*/*',
      'Content-Type': 'application/json'
    }
  })
}