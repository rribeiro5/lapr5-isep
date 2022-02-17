import Reaction from "../Reaction/Reaction";
import {updateReactionComment} from "../../services/PostService";
import './CommentView.css'

export default props => {
    const { comment, user, updateFeed } = props

    return (
        <div className="comment-line">
            <p className="comment-text"><span style={{ fontWeight: 500 }}>{user.name}:</span> {comment.text}</p>
            <Reaction publicationId={comment.id} updateFeed={updateFeed} reactions={comment.reactions} handlerReaction={updateReactionComment} />
        </div>
    )
}