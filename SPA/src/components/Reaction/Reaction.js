
import {AiOutlineHeart,AiOutlineDislike} from 'react-icons/ai'
import {useContext, useEffect, useState} from "react";
import {Context} from "../../context/loggedUser";
import CreateReactionDTO from "../../model/Post/CreateReactionDTO";
import {updateReaction} from "../../services/PostService";
import { useTranslation } from 'react-i18next';
import {failedNotification,successNotification} from "../../utils/ToastContainerUtils";
import "./Reaction.css"


export default function  Reaction({publicationId,reactions,updateFeed,handlerReaction}){
    const {loggedUser} = useContext(Context)
    const reactionMap = new Map()
    reactionMap.set("LIKE",0)
    reactionMap.set("DISLIKE",0)
    // meter handler para atualizar publication quando se da like ou nao
    
    const [likeActive,setLikeActive] = useState(false)
    const [dislikeActive,setDislikeActive] = useState(false)
    //const activeColor 
    
    const[mapReactions,setMapReactions] = useState(reactionMap)
    const { t } = useTranslation()
    
    let likeStyle = {
        color: likeActive && getComputedStyle(document.documentElement).getPropertyValue("--main-theme-color")
    }

    let dislikeStyle = {
        color: dislikeActive && getComputedStyle(document.documentElement).getPropertyValue("--main-theme-color")
    }
    
    
    useEffect(()=>{
        const newMap = new Map()
        newMap.set("LIKE",0)
        newMap.set("DISLIKE",0)
        if(reactions.length!==0){
            setLikeActive(false)
            setDislikeActive(false)
            reactions.map(reaction => {
                let typeReaction = reaction.reaction
                if(reaction.userId === loggedUser.id){
                    typeReaction==="LIKE"? setLikeActive(true):setDislikeActive(true)
                }
                let value =  newMap.get(typeReaction)
                if(value!==undefined){
                    newMap.set(typeReaction,value + 1)
                }else{
                    newMap.set(typeReaction,0)
                }
            })
        }
        setMapReactions(newMap)
    },[reactions])
    
    
    
    function handleClickPublication (typeReaction){
        typeReaction==="LIKE"? setLikeActive(!likeActive):setDislikeActive(!dislikeActive)
        const createReactionDTO = new CreateReactionDTO(publicationId,loggedUser.id,typeReaction)
        handlerReaction(createReactionDTO)
            .then(()=>successNotification(t('reaction.success')))
            .catch(() => failedNotification( t('reaction.error')) )
            .finally(()=>updateFeed())
    }
    
    return (
        <div className="reaction-main-container">
            <div className="reaction-like">
                <AiOutlineHeart  onClick={()=>handleClickPublication("LIKE")} />
                <span style={likeStyle}>{mapReactions.get("LIKE")}</span>
            </div>
            <div className="reaction-dislike">
                <AiOutlineDislike onClick={()=>handleClickPublication("DISLIKE")}/>
                <span style={dislikeStyle}>{mapReactions.get("DISLIKE")}</span>
            </div>
        </div>
    )
}