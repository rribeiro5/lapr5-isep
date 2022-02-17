import { useTranslation } from 'react-i18next'
import {BsArrowDown} from 'react-icons/bs'
import './TrajectViewer.css'
export default function TrajectViewer(props){
    
    const{caminho,forcaResultante} = props

    const { t } = useTranslation()
    
    function showTraject (){
        return(
            <div className="TrajectViewerContainer">
                {forcaResultante && <h3>{t('trajects.strength')} {forcaResultante}</h3>}
                    <h3>{t('trajects.traject')}</h3> 
                    <div className="Traject">
                        {caminho.map( t=> <div key={t} ><p>{t} </p><BsArrowDown /></div>)}
                    </div>
            </div>
        )   
    }
    
    
    
    return(
        caminho.length!==0 && showTraject()
    )
    
}