import React, { useState, useEffect } from 'react'

import { useTranslation } from 'react-i18next'
import { feedPosts } from '../../services/PostService'
import PostView from '../PostView/PostView'

import './FeedPosts.css'

export default props => {
    const { user } = props
    const userId = user !== undefined ? user.id : ""
    
    const { t } = useTranslation()

    const [state, setState] = useState({
        feed: [],
    })

    useEffect(() => {
        fetchPosts()
        /*
        const timer = setTimeout(()=>{
            fetchPosts()
        },1500)
        return () => clearTimeout(timer)*/
    },[props])

    useEffect(() => {
        const timer = setTimeout(()=>{
            fetchPosts()
        },1500)
        return () => clearTimeout(timer)
    })
    
    function fetchPosts(){
        if (userId !== "") {
            feedPosts(userId)
                .then(res => setState({ ...state, feed: res.data }))
                .catch(err => console.error(err))
        }
    }

    return (
        <div className="feed-container">
            {state.feed.map(post => <PostView key={post.id} updateFeed={fetchPosts} post={post} user={user} />)}
        </div>
    )
}