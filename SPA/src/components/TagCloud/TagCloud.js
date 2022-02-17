import React, { useState, useEffect, useContext, useReducer } from "react"
import { useTranslation } from "react-i18next";

import TagCloud from "react-tag-cloud";
import randomColor from "randomcolor";
//import { TagCloud } from 'react-tagcloud'

import { Context } from "../../context/loggedUser";
import { tagCloudAllConns, tagCloudAllUsers, tagCloudOfUser, tagCloudOfUserConns } from "../../services/TagCloudService";

import './TagCloud.css'

const ALL_USERS = "all-users"
const ALL_CONNS = "all-conns"
const LOGGED_USER = "logged-user"
const LOGGED_CONNS = "logged-conns"

export default props => {
    const { loggedUser } = useContext(Context)

    const { t } = useTranslation()

    const [state, setState] = useState({
        tagCloud: [],
        max: 0,
        min: 0,
        type: "default",
    })

    const [_, forceUpdate] = useReducer((x) => x + 1, 0);

    useEffect(() => {
        props.nameCurrentComponent(t('tagcloud.title'))

        setInterval(() => {
            forceUpdate()
        }, 5000);
    }, [])

    const setTagCloud = (request, type) => {
        request
            .then(res => {
                setState({ ...state,
                    tagCloud: res.data,
                    max: Math.max(...res.data.map((e) => e.count)),
                    min: Math.min(...res.data.map((e) => e.count)),
                    type: type
                })
            })
            .catch(err => console.error(err))
    }

    const handleSelect = (evt) => {
        const type = evt.target.value
        switch (type) {
            case ALL_USERS:
                setTagCloud(tagCloudAllUsers(), type)
                break;
            case ALL_CONNS:
                setTagCloud(tagCloudAllConns(),type)
                break;
            case LOGGED_USER:
                setTagCloud(tagCloudOfUser(loggedUser.id), type)
                break;
            case LOGGED_CONNS:
                setTagCloud(tagCloudOfUserConns(loggedUser.id), type)
                break;
            default:
                setState({ ...state, tagCloud: [], type: evt.target.value })
        }
    }

    const calcFontSize = (count, minSize, maxSize) => {
        const diff = state.max - state.min
        if (diff === 0) {
            return maxSize
        }
        const div = (count - state.min) / diff
        return div * (maxSize - minSize) + minSize
    }

    const getRotation = () => {
        const rotations = [0, 0, 0, 0, 0, 0, 90, 270]
        return rotations[Math.floor(Math.random() * rotations.length)]
    }

    return (
        <div className="tagcloud-container">
            <select value={state.type} onChange={handleSelect} className="tagcloud-type">
                <option value="default">{t('tagcloud.default')}</option>
                <option value={ALL_USERS}>{t('tagcloud.allusers')}</option>
                <option value={ALL_CONNS}>{t('tagcloud.allconns')}</option>
                <option value={LOGGED_USER}>{t('tagcloud.loggeduser')}</option>
                <option value={LOGGED_CONNS}>{t('tagcloud.loggedconns')}</option>
            </select>
            {/* <TagCloud tags={state.tagCloud} minSize={12} maxSize={35} /> */}
            <TagCloud
                    role="tags"
                    className="tag-cloud-tags" 
                    style={{
                        fontFamily: 'sans-serif',
                        fontWeight: 'bold',
                        color: () => randomColor({ luminosity: 'light' }),
                        padding: 5,
                        width: '100%',
                        height: '250px'
                    }}>
                {state.tagCloud.map((tag) => <p key={new Date().valueOf()} style={{ fontSize: calcFontSize(tag.count, 12, 50) }} rotate={getRotation()}>{tag.value}</p>)}
            </TagCloud>
        </div>
    )
}