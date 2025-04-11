import React, { useEffect, useState } from 'react';
import './App.css';

const PORT = 7040;
const API_URL = `https://localhost:${PORT}/api/ToDoItem`;

function App() {
  const [toDos, setToDos] = useState([]);
  const [newToDo, setNewToDo] = useState("");

  const fetchToDos = async () => {
    const result = await fetch(API_URL);
    const data = await result.json();
    // console.log("Data: ", data);
    setToDos(data);
  };

  const addToDo = async () => {
    if (!newToDo.trim()) return;
    await fetch(API_URL, {
      method: "POST", 
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ task: newToDo })
    });

    setNewToDo("");
    fetchToDos();
  };

  const deleteToDo = async (id) => {
    await fetch(`${API_URL}/${id}`, { method: "DELETE" });
    fetchToDos();
  };

  const toggleComplete = async (id, currentStatus) => {
    const taskToUpdate = toDos.find(todo => todo.id === id);
    
    console.log(taskToUpdate);
    
    if (!taskToUpdate) console.log("Task not found");
    
    const updatedToDo = {
      id: taskToUpdate.id,
      task: taskToUpdate.task,
      isCompleted: !currentStatus
    };

    await fetch(`${API_URL}/${taskToUpdate.id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(updatedToDo)
    });

    fetchToDos();
  };

  const editTask = async (id, newTask) => {
    const taskToUpdate = toDos.find(todo => todo.id === id);

    if (!taskToUpdate) console.log("Task not found");
    
    const updatedToDo = {
      id: taskToUpdate.id,
      task: newTask,
      isCompleted: taskToUpdate.isCompleted
    };

    await fetch(`${API_URL}/${taskToUpdate.id}`, {
      method: "PUT", 
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(updatedToDo)
    });

    fetchToDos();
  };

  useEffect(() => {
    fetchToDos();
  }, []);

  return (
    <div className="container">
      <h1 className="heading">Simple To Do List</h1>
      <div className="inputGroup">
        <input
          className="input"
          value={newToDo}
          onChange={e => setNewToDo(e.target.value)}
          placeholder='Add here'
        />
        <button onClick={addToDo} className="addButton">Add</button>
      </div>
      <ul className="toDoList"> 
        {toDos.map(toDo => (
          <li key={toDo.id} className="toDoItem">
            <div className="circle"></div>
            <span className="title">{toDo.task}</span>
            <button onClick={() => toggleComplete(toDo.id, toDo.isCompleted)} className="completeButton">
                {toDo.isCompleted ? "Undo" : "Complete"}
            </button>
            <button onClick={() => {
              const newTask = prompt("Edit task", toDo.task); 
              if (newTask && newTask !== toDo.task) {
                editTask(toDo.id, newTask);
              }
            }} className="editButton">
              edit
            </button>
            
            <button onClick={() => deleteToDo(toDo.id)} className="deleteButton">
                ❌
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
