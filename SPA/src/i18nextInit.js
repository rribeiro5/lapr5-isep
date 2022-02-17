import i18n from 'i18next'
import { initReactI18next } from 'react-i18next'
import Backend from 'i18next-xhr-backend'

import translationEN from './assets/translations/en.json'
import translationPT from './assets/translations/pt.json'

const fallbackLng = ["en"]

const resources = {
    en: {
        translation: translationEN
    },
    pt: {
        translation: translationPT
    }
}

i18n
    .use(Backend)
    .use(initReactI18next)
    .init({
        resources,
        fallbackLng,
        lng: fallbackLng,

        debug: false,

        interpolation: {
            escapeValue: false
        }
    })

export default i18n