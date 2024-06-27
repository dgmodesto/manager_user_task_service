'use client'

import { TasksService } from '@/services/tasks';
import Link from 'next/link';
import { redirect, useSearchParams } from 'next/navigation';
import { useState } from 'react';

export default function NewTaskForm() {
  const searchParams = useSearchParams();

  const error = searchParams.get('error');
  const [task, setTask] = useState({
    user: '',
    date: '',
    startTime: '',
    endTime: '',
    subject: '',
    description: '',
    id: '',
    createdAt: '',
    version: 0,
  });

  const handleChange = (e: any) => {
    const { name, value } = e.target;
    setTask((prevTask) => ({
      ...prevTask,
      [name]: value,
    }));
  };



  function formatDate(date: any) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0'); // adiciona um zero à esquerda se for menor que 10
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    // Monta a string no formato desejado
    return `${year}-${month}-${day}T${hours}:${minutes}:00.000Z`;
  }

  function save(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const formData = new FormData(e.currentTarget)

    const dateSelected = formData.get('date')?.toString()
    const [year, month, day] = dateSelected!.split("-");

    var DateTask = new Date(Number(year), Number(month), Number(day), 0, 0);

    var startTime = formData.get('startTime')?.toString();
    const [hours, minutes] = startTime!.split(":");
    const startDate = new Date(Number(year), Number(month), Number(day), Number(hours), Number(minutes));

    var endTime = formData.get('endTime')?.toString();
    const [endHours, endMinutes] = endTime!.split(":");
    const endDate = new Date(Number(year), Number(month), Number(day), Number(endHours), Number(endMinutes));



    const data = {
      user: formData.get('user'),
      date: formatDate(DateTask),
      startTime: formatDate(startDate),
      endTime: formatDate(endDate),
      subject: formData.get('subject'),
      description: formData.get('description'),
    }



    TasksService.New(data).then((response) => {
      if (response === null) {
        console.log(`HTTP error! status: ${response.status}`);
        return;
      }

      setTask({
        user: '',
        date: '',
        startTime: '',
        endTime: '',
        subject: '',
        description: '',
        id: '',
        createdAt: '',
        version: 0,
      });
    })

  }

  return (
    <div className="max-w-md mx-auto">
      <div className='flex justify-center w-full m-2 p-2'>
        <h1 className="text-2xl font-bold mb-4">New User Tasks</h1>
      </div>
      <form onSubmit={save} className="space-y-4">
        <div>
          <label htmlFor="id" className="block text-sm font-medium text-gray-700">ID</label>
          <input
            type="text"
            disabled
            id="id"
            name="id"
            value={task.id}
            onChange={handleChange}
            className="input input-bordered w-full"
          />
        </div>
        <div>
          <label htmlFor="user" className="block text-sm font-medium text-gray-700">User</label>
          <input
            type="text"
            id="user"
            name="user"
            value={task.user}
            onChange={handleChange}
            className="input input-bordered w-full"
            required
          />
        </div>
        <div>
          <label htmlFor="date" className="block text-sm font-medium text-gray-700">Date</label>
          <input
            type="date"
            id="date"
            name="date"
            value={task.date}
            onChange={handleChange}
            className="input input-bordered w-full"
            required
          />
        </div>
        <div>
          <label htmlFor="startTime" className="block text-sm font-medium text-gray-700">Start Time</label>
          <input
            type="time"
            id="startTime"
            name="startTime"
            value={task.startTime}
            onChange={handleChange}
            className="input input-bordered w-full"
            required
          />
        </div>
        <div>
          <label htmlFor="endTime" className="block text-sm font-medium text-gray-700">End Time</label>
          <input
            type="time"
            id="endTime"
            name="endTime"
            value={task.endTime}
            onChange={handleChange}
            className="input input-bordered w-full"
            required
          />
        </div>
        <div>
          <label htmlFor="subject" className="block text-sm font-medium text-gray-700">Subject</label>
          <input
            type="text"
            id="subject"
            name="subject"
            value={task.subject}
            onChange={handleChange}
            className="input input-bordered w-full"
            required
          />
        </div>
        <div>
          <label htmlFor="description" className="block text-sm font-medium text-gray-700">Description</label>
          <textarea
            id="description"
            name="description"
            value={task.description}
            onChange={handleChange}
            className="input input-bordered w-full"
            rows={4}
            required
          ></textarea>
        </div>

        <div className='flex justify-between'>
          <Link className='btn btn-secondary' href={'/task'}>Back</Link>
          <button type="submit" className="btn btn-primary">Enviar</button>
        </div>
      </form>
    </div>
  )
}