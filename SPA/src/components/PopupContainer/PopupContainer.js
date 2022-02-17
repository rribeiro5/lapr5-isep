import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';

export default function PopupContainer(props){
    const{Component,title} = props
    
    
    return(
        <Popup trigger={<button className="popup-button" type="button"> {title} </button>} modal
               nested>
            {close=>(
                <div className="modal">
                    <button className="close" onClick={close}>
                        &times;
                    </button>
                    <div className="header"> {title} </div>
                    {Component}
                </div>
            )}
        </Popup>
    )
}