import 'bootstrap/dist/css/bootstrap.css';
// Put any other imports below so that CSS from your
// components takes precedence over default styles.
import React, { useState } from 'react';
import { Route, Routes, Navigate, useNavigate } from 'react-router-dom';
import RegistrationComponent from './components/RegistrationComponent.js';
import MainCopmoment from './components/MainCompoment.js';
import PostCreateForm from './components/PostCreateForm.js';
import EnterComponent from './components/EnterComponent.js';
import PostUpdateComponent from './components/PostUpdateComponent.js';

export default function App() {
    const navigate = useNavigate();
    const [post, setPost] = useState({}); 
    const handleSubmit = (formData) => {

        const authorization = {
            Login: formData.login,
            Password: formData.password
        };

        const url = "https://localhost:5001/api/user/authorization";

        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(authorization)

        })
            .then(response => response.json())
            .then(responseFromServer => {
                if(responseFromServer.success){
                    sessionStorage.setItem("token", responseFromServer.token);
                    sessionStorage.setItem("userId", responseFromServer.userId);
                    navigate("/")
                }
                else{
                    alert(responseFromServer.message);
                }
            })
            .catch((error) => {
                alert(error);
            });
    };

    const updatePost = (formData) => {    
        const postToUpdate = {
            Title: formData.title,
            Content: formData.content,
            PostId:  formData.postId
        };
    
        const url = "https://localhost:5001/api/posts/" + formData.postId;
    
        fetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                "Authorization": "Bearer " + sessionStorage.getItem("token")
            },
            body: JSON.stringify(postToUpdate)
    
        })
        .then(response => {
            if(response.status === 204){
                navigate("/");
            }
        })
        .catch((error) => {
            alert(error);
        });
    }

    return (
        <Routes>
            <Route path='/login' element={sessionStorage.getItem("token") ? <Navigate to={"/"}/> : <EnterComponent handleSubmit={handleSubmit}/>}/>
            <Route path='/registration' element={<RegistrationComponent/>}/>
            <Route path='/' element={sessionStorage.getItem("token") ? <MainCopmoment setPost={setPost}/> : <Navigate to={"/login"}/> }/> 
            <Route path='/addPost' element={<PostCreateForm/>}/>
            <Route path='/updatePost' element={<PostUpdateComponent  post={post} updatePost={ updatePost }/>}/>
        </Routes>
    );
}