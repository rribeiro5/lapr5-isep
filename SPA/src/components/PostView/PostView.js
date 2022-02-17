import useCollapse from "react-collapsed";
import { useTranslation } from "react-i18next"
import {BiMessageRounded} from 'react-icons/bi';
import CommentView from "../CommentView/CommentView";
import Reaction from "../Reaction/Reaction";
import {updateReactionPost} from "../../services/PostService";
import Comment from "../Comment/Comment"; 

import './PostView.css'

export default props => {
    const { post, user } = props

    const { t } = useTranslation()

    const { getCollapseProps, getToggleProps } = useCollapse()

    const creation = new Date(post.creationDateTime)
    const date = (creation.getUTCFullYear() + "/" + 
        ("0" + (creation.getUTCMonth()+1)).slice(-2) + "/" +
        ("0" + creation.getUTCDate()).slice(-2))
    const time = creation.toLocaleTimeString('pt-PT')

    return (
        <div className="post-container">
            <div className="post-header">
                <p className="post-header-text">{t('feedposts.publishedby')}<span style={{ fontWeight: 700 }}>{user.name}</span>{t('feedposts.ondate')}<span style={{ fontWeight: 400 }}>{date}</span>{t('feedposts.attime')}<span style={{ fontWeight: 400 }}>{time}</span></p>
            </div>
            <div className="post-body">
                <p className="post-body-text">{post.text}</p>
                {post.tags.length>0 &&<p className="post-body-tags"><span style={{ fontWeight: 500 }}>Tags: </span>{post.tags.join(", ")}</p>}
                <div className="post-actions">
                    <div className="btn-comments-container" {...getToggleProps()}>
                        <BiMessageRounded />
                        <span className="comments-count">{post.comments.length}</span>
                    </div>
                    <Reaction updateFeed={props.updateFeed} publicationId={post.id} reactions={post.reactions} handlerReaction={updateReactionPost}  />
                </div>
            </div>
            <div className="post-comments">
                <Comment postId={post.id} />
                {post.comments.length<=0 && <p style={{ marginTop: '10px' }}>{t('comment.first')}</p>}
                <div className="comments-container" {...getCollapseProps()}>
                    { post.comments.map((comment) => <CommentView comment={comment} user={comment.user} updateFeed={props.updateFeed} />) }
                </div>
            </div>
        </div>
    );
}