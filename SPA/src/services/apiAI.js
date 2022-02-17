import axios from 'axios'

export default axios.create({ // TODO Configurable
    baseURL: 'https://lapr5g50ai.azurewebsites.net/'
})