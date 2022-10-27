import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';

export default function PostCreateForm(props) {
    const [formData, setFormData] = useState({});
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        const postToCreate = {
            Title: formData.title,
            Content: formData.content,
            UserId: sessionStorage.getItem("userId")
        };

        const url = "https://localhost:5001/api/posts/addPost";

        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                "Authorization": "Bearer " + sessionStorage.getItem("token")
            },
            body: JSON.stringify(postToCreate)

        })
            .then(response => response.json())
            .then(responseFromServer => {
                navigate("/getPosts")
            })
            .catch((error) => {
                console.log(error);
                alert(error);
            });
    };

    return (
        <form className="w-100 px-5">
            <h1 className="mt-5">Create new post</h1>

            <div className="mt-5">
                <label className="h3 form-label">Post title</label>
                <input value={formData.title} name="title" type="text" className="form-control" onChange={handleChange} />
            </div>

            <div className="mt-4">
                <label className="h3 form-label">Post content</label>
                <input value={formData.content} name="content" type="text" className="form-control" onChange={handleChange} />
            </div>

            <button onClick={handleSubmit} className="btn btn-dark btn-lg w-100 mt-5">Submit</button>
            <button onClick={() => navigate("/")} className="btn btn-secondary btn-lg w-100 mt-3">Cancel</button>
        </form>
    );
}
