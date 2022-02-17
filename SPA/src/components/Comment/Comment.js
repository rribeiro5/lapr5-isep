import React, {useContext,useState,useEffect} from "react";
import "./Comment.css"
import {createComment} from '../../services/PostService';
import CreateCommentDTO from "../../model/Post/CreateCommentDTO";
import {failedNotification, successNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from 'react-i18next';
import { Context } from "../../context/loggedUser";

export default function Comment(props) {
    const {t} = useTranslation()
    const {loggedUser} = useContext(Context)

    const [comment, setCommentData] = useState({
       // userId:loggedUser.id,
       // postId:props.postId,
        text:"",
       // reactions:[]
    })

    useEffect(()=>{
        setCommentData({text:""})
    },[])

    function submit(e){
        e.preventDefault()
        if(comment.text.length === 0){
            failedNotification(t('comment.empty'))
            return;
        }
        const {text}=comment
        const dto=new CreateCommentDTO(props.postId,loggedUser.id,text,[])
        console.log(dto)
        createComment(dto)
            .then(()=>successNotification(t('comment.success')))
            .then(()=>setCommentData({text:""}))
            .catch(err=>console.error(err.response))
    }

    function updateTextArea(e){
        const value=e.target.value
        setCommentData(prevState => ({...prevState,"text":value}))
    }

    return (
        <div className="comment-main-container">
            <form className="comment-form-container" onSubmit={submit}>
                <textarea
                    className="comment-form-input-text"
                    placeholder={t('comment.add')}
                    rows={1}
                    value={comment.text}
                    onChange={updateTextArea}>
                </textarea>
                <input type="submit" 
                    value={t('formdefaults.submit')}
                    className="comment-submit" />
            </form>
        </div>
    )
}
