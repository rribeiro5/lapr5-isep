import React, {useState, useEffect, useContext} from "react"
import { useTranslation } from "react-i18next";
import Popup from "reactjs-popup";

import Connection from "../../model/Connection/Connection"
import { getConnectionsOfUser } from "../../services/ConnectionService"

import {Context} from "../../context/loggedUser";
import UpdateConnection from "../UpdateConnection/UpdateConnection";
import ProfilePreview from "../ProfilePreview/ProfilePreview";
import {failedNotification} from "../../utils/ToastContainerUtils";

import './Connections.css'
import FriendsInCommon from "../FriendsInCommon/FriendsInCommon";

export default props => {
    
    const {loggedUser} = useContext(Context)

    const { t } = useTranslation()

    const [state, setState] = useState({
        connections: [],
    })

    useEffect(() => {
        props.nameCurrentComponent(t('connections.title'))
        getConnectionsOfUser(loggedUser.id)
            .then(res => setState({ 
                ...state, 
                connections: res.data.connections.map((conn) => 
                    new Connection(conn.id, conn.oUser, conn.origUser, conn.dUser, conn.destUser, conn.connectionStrength, conn.relationshipStrength, conn.tags))
            }))
            .catch(err => failedNotification(err.response.data))
    }, [])

    const connComponents = () => {
        let arr = []
        for (const conn of state.connections) {
            const comp = (
                <div key={conn.id} className="connection">
                    <div className="connInfo">
                        <div className="Preview">
                            <ProfilePreview user={conn.destUser}/>
                        </div>
                        <div className="info">
                            <h3>{conn.destUser.name} {/*({conn.destUser.email})*/}</h3>
                            <p>{t('connections.connstrength')} {conn.connectionStrength} </p>
                            <p>{t('connections.relstrength')} {conn.relStrength} </p>
                        </div>
                    </div>
                    <div className="connAction">
                        <Popup trigger={<button>{t('connections.edtbutton')}</button>} modal
                                nested>
                            {close => (
                                <div className="modal">
                                    <button className="close" onClick={close}>
                                        &times;
                                    </button>
                                    <div className="header">{t('editconnections.title')}</div>
                                    <UpdateConnection connection={conn} />
                                </div>
                            )}
                        </Popup>
                        <FriendsInCommon destUserId={conn.destId.value} />
                    </div>
                </div>
            )
            arr.push(comp)
        }
        return arr
    }

    return (
        <div className="connContainer">
            {connComponents()}
        </div>
    )

}