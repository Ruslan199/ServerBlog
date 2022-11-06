import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';

function updateData(formData) {
    const postToUpdate = {
        Title: formData.title,
        Content: formData.content,
        UserId: sessionStorage.getItem("userId")
    };

    const url = "https://localhost:5001/api/post" + formData.postId;

    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(postToUpdate)

    })
    .then(response => response.json())
    .then(responseFromServer => {
        if(responseFromServer.token !== ""){
            //navigate("/")
        }
    })
    .catch((error) => {
        console.log(error);
        alert(error);
    });
}

export default function PostUpdateComponent(props) {
    
    const gg = Object.freeze({
        title: props.post.title,
        content: props.post.content,
        postId: props.post.postId
    }); 

   console.log(gg);

    const [formData, setFormData] = useState(gg);

    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };

    return (
        <form className="w-100 px-5">
            <h1 className="mt-5">Редактирование текущего поста</h1>

            <div className="mt-5">
                <label className="h3 form-label">Заголовок</label>
                <input value={formData.title} name="title" type="text" className="form-control" onChange={handleChange} />
            </div>

            <div className="mt-4">
                <label className="h3 form-label">Контент</label>
                <input value={formData.content} name="content" type="text" className="form-control" onChange={handleChange} />
            </div>
            
            <button onClick={() => { updateData(formData)}} className="btn btn-dark btn-lg w-100 mt-5">Обновить</button>
            <button onClick={() => navigate("/")} className="btn btn-secondary btn-lg w-100 mt-3">Отмена</button>
        </form>
    );
}
