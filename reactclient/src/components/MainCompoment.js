import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

function RenderPostTable({posts, setPosts}) {
  return (
    <div className="table-responsive mt-5">
        <table className="table table-bordered border-dark">
            <thead>
                <tr>
                  <th scope="col">PostId PK</th>
                  <th scope="col">Title</th>
                  <th scope="col">Content</th>
                  <th scope="col">CreatedOn</th>
                  <th scope="col">CRUD Operations</th>
                </tr>
            </thead>
            <tbody>
              {posts.map((post) => (
                <tr key={post.postId}>
                  <th scope="row">{post.postId}</th>
                  <td>{post.title}</td>
                  <td>{post.content}</td>
                  <td>{post.createdOn}</td>
                  <td>
                    <button className="btn btn-dark btn-lg mx-3 my-3">Update</button>
                    <button className="btn btn-secondary btn-lg">Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
        </table>

        <button onClick={() => setPosts([])} className="btn btn-dark btn-lg w-100">Empty react posts array</button>
    </div>
  );
}

export default function MainCopmonent() {
  
  const [posts, setPosts] = useState([]);
  const navigate = useNavigate();

  function exit() {
    sessionStorage.removeItem("token");
    sessionStorage.removeItem("user");
    navigate("/");
  }
  function getPosts() {
    const url = "https://localhost:5001/api/posts/getUserPosts";

    fetch(url,{
      headers: {
        'Content-Type': 'application/json',
        "Authorization": "Bearer " + sessionStorage.getItem("token")
      }
    })
      .then(response => response.json())
      .then(postsFromServer => {
        console.log(postsFromServer);
        setPosts(postsFromServer.posts);
      })
      .catch((error) => {
        console.log(error);
        alert(error);
      });
  }


  return (
    <div className="container">
      <div className="row min-vh-100">
        <div className="col d-flex flex-column justify-content-center align-items-center">
          <h1>ASP.Net  Core REact</h1>

          <div className="mt-5">
                <button onClick={ exit } className="btn btn-dark btn-lg w-100">Выйти</button>



               <button onClick={ getPosts } className="btn btn-dark btn-lg w-100">Get Posts From Server</button>
               <button onClick={() => navigate("/addPost")} className="btn btn-secondary btn-lg w-100 mt-4">Create New Post</button>
          </div>


          {posts.length > 0 && <RenderPostTable posts={posts} setPosts={setPosts}/>}
        </div>
      </div>    
    </div>
  );
}