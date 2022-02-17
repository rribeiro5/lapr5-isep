import React, { useState, useEffect } from "react"
import { useTranslation } from "react-i18next";

import { FaRegQuestionCircle } from 'react-icons/fa'

import { getSocialNetworkDimension } from "../../services/UserService";

import './SocialNetworkInfo.css'

export default () => {

    const { t } = useTranslation()

    const [state, setState] = useState({
        dimension: 0,
    })

    useEffect(() => {
        getSocialNetworkDimension()
            .then(res => setState({ ...state, dimension: res.data.value }))
            .catch(err => console.error(err))
    }, [])

    return (
        <>
            <div className="sninfo">
                <FaRegQuestionCircle size={20} />
                
            </div>
            <div className="sninfo-text">
                <p className="sninfo-title">{t('sninfo.title')}</p>
                <p className="sninfo-info"><span style={{ fontWeight: "bold" }}>{t('sninfo.dimension')}</span> {state.dimension} {t('sninfo.users')}</p>
            </div>
        </>
        
    )
}