import { toast } from 'react-toastify';


export function successNotification(message){
    const customId = "custom-id-yes";
    toast.success(message,{
        position: "top-center",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
        toastId: customId,
        className:"Toastify__progress-bar--info"
    })
}

export function failedNotification(message){
    const customId = "custom-id-no";
    toast.error(message, {
        position: "top-center",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        toastId: customId,
        progress: undefined,
        className:"Toastify__progress-bar--error",
    })
}