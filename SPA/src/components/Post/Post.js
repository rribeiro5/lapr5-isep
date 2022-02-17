import React, {useState,useEffect} from "react";
import TagsInput from "react-tagsinput";
import "./Post.css"

import {createPost} from '../../services/PostService';
import CreatePostDTO from "../../model/Post/CreatePostDTO";
import {failedNotification, successNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from 'react-i18next';


export default function Post(props){
    const { t } = useTranslation()
    
    const MIN_TEXT_SIZE=1
    const MAX_TEXT_SIZE=10_000
    const [post, setPostData] = useState({
        userId:props.userId,
        text:"",
        tags:[]
    })
    
    useEffect(()=>{
        setPostData({userId: props.userId,text:"",tags:[]})
    },[])
    
    function submit(e){
        e.preventDefault()
        if(post.text.length<MIN_TEXT_SIZE || post.text.length >MAX_TEXT_SIZE){
            failedNotification(t('post.illegalTextLength'))
            return;
        }
        const {userId,text,tags}=post
        const dto=new CreatePostDTO(userId,text,tags)
        createPost(dto)
            .then(()=>successNotification(t('post.success')))
            .then(()=>setPostData({userId:props.userId,text:"",tags:[]}))
            .catch(err=>console.error(err.response))

    }
    
    function updateTextArea(e){
        const value=e.target.value
        setPostData(prevState => ({...prevState,"text":value}))
    }


return(
        <div className="post-main-container">
            <div className="post-form-container">
                <textarea
                    id="post-area"
                    className="post-form-input-text"
                    placeholder={t('post.placeholder')}
                    value={post.text}
                onChange={updateTextArea}>
                </textarea>
                <TagsInput
                    className="react-tagsinput"
                    value={post.tags}
                    addKeys={[9,13,32]}
                    onChange={(tags)=>setPostData(prevData => {return {...prevData,tags}})}
                    required
                />
                <button
                    id="submit-post-btn"
                    className="submitButton"
                    onClick={submit}
                >{t('post.submitButton')}</button>
            </div>
        </div>
    )
}