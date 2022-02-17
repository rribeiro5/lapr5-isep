import './UserSugestionSidebar.css'
import ProfilePreview from "../ProfilePreview/ProfilePreview";
import DirectConnectionRequest from "../DirectConnectionRequest/DirectConnectionRequest";



export default function (props){
    const{user} = props
    return (
        <div key={user.id} className="sugestion">
            <div className="user-info">
                <ProfilePreview  user={user} />
                <h3>{user.name}</h3>
            </div>
            <DirectConnectionRequest  dest={user} />
        </div>
    )
    
}