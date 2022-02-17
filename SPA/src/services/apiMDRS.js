import axios from 'axios'

export default axios.create({ // TODO Configurable
    baseURL: 'https://lapr5g50.azurewebsites.net/'
    //baseURL: 'https://localhost:5001/'
})