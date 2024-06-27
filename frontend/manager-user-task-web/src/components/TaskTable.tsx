'use client'

import { TasksService } from '@/services/tasks';
import Link from 'next/link';
import { redirect } from 'next/navigation';
import { useState, useEffect } from 'react';

interface Task {
  user: string;
  date: string;
  startTime: string;
  endTime: string;
  subject: string;
  description: string;
  id: string;
  createdAt: string;
  version: number;
}

interface TaskGroup {
  date: string;
  userTasks: Task[];
  createdAt: string;
  version: number;
}

interface ApiResponse {
  success: boolean;
  data: {
    currentPage: number;
    pageSize: number;
    recordsInPage: number;
    totalPages: number;
    totalRecords: number;
    filterBy: string;
    sorting: string;
    records: TaskGroup[];
  };
  errors: any[];
}

export default function TasksTable() {
  const [data, setData] = useState<ApiResponse | null>(null);
  const [page, setPage] = useState(1);
  const [error, setError] = useState<string | null>(null);
  const [expandedDates, setExpandedDates] = useState<string[]>([]);
  const pageSize = 10;

  useEffect(() => {
    TasksService.List(page, pageSize).then((response) => {
      if (!response.success) {
        console.log(`HTTP error! status: ${response.status}`);
        return;
      }
      const result = response;
      setData(result);
      setError(null);

    })

  }, [page]);

  const handleEdit = (id: string) => {
    // Implement the edit functionality here
    redirect(`/task/update-task/${id}`);
  };

  const handleRemove = async (id: string) => {
    try {
      TasksService.Remove(id).then((response) => {
        if (response) {
          // Refresh the task list after a successful removal
          setPage(1); // Reset to the first page or keep the current page
          TasksService.List(page, pageSize).then((response) => {
            const result = response;
            setData(result);
            setError(null);
          });
        } else {
          setError('Failed to remove the task');
        }
      });

    } catch (error) {
      setError('Failed to remove the task');
    }
  };


  const toggleExpand = (date: string) => {
    setExpandedDates((prev) =>
      prev.includes(date) ? prev.filter(d => d !== date) : [...prev, date]
    );
  };

  if (error) {
    return <div className="container mx-auto px-4">Error: {error}</div>;
  }

  if (!data) {
    return <div className="container mx-auto px-4">Loading...</div>;
  }

  return (
    <div className="container mx-auto px-4">
      <div className='flex justify-between m-2 p-2'>
        <h1 className="text-2xl font-bold mb-4">User Tasks</h1>
        <Link className='btn btn-primary' href={'task/new-task'}>New Task</Link>
      </div>

      {data.data.records.map((group) => (
        <div key={group.date}>
          <div
            className="cursor-pointer py-2 px-4 border-b bg-gray-200"
            onClick={() => toggleExpand(group.date)}
          >
            {new Date(group.date).toLocaleDateString()}
          </div>
          {expandedDates.includes(group.date) && (
            <div className="bg-white border-b">
              <table className="min-w-full bg-white border border-gray-200">
                <thead>
                  <tr>
                    <th className="py-2 px-4 border-b text-center">User</th>
                    <th className="py-2 px-4 border-b text-center">Start Time</th>
                    <th className="py-2 px-4 border-b text-center">End Time</th>
                    <th className="py-2 px-4 border-b text-center">Subject</th>
                    <th className="py-2 px-4 border-b text-center">Description</th>
                    <th className="py-2 px-4 border-b text-center">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {group.userTasks.map((task) => (
                    <tr key={task.id}>
                      <td className="py-2 px-4 border-b  text-center">{task.user}</td>
                      <td className="py-2 px-4 border-b  text-center">{new Date(task.startTime).toLocaleTimeString([], { timeZone: 'UTC', hour12: false })}</td>
                      <td className="py-2 px-4 border-b  text-center">{new Date(task.endTime).toLocaleTimeString([], { timeZone: 'UTC', hour12: false })}</td>
                      <td className="py-2 px-4 border-b  text-center">{task.subject}</td>
                      <td className="py-2 px-4 border-b  text-center">{task.description}</td>
                      <td className="py-2 px-4 border-b  text-center">

                        <Link className='btn bg-yellow-500 text-white py-1 px-2 rounded mr-2 w-24' href={`task/update-task/${task.id}`}>Edit</Link>
                        <button
                          className="btn  bg-red-500 text-white py-1 px-2 rounded  w-24"
                          onClick={() => handleRemove(task.id)}>
                          Remove
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      ))}
      <div className="flex justify-between mt-4">
        <button
          className="bg-blue-500 text-white py-2 px-4 rounded"
          onClick={() => setPage(page > 1 ? page - 1 : 1)}
          disabled={page === 1}
        >
          Previous
        </button>
        <span>
          Page {data.data.currentPage} of {data.data.totalPages}
        </span>
        <button
          className="bg-blue-500 text-white py-2 px-4 rounded"
          onClick={() => setPage(page < data.data.totalPages ? page + 1 : data.data.totalPages)}
          disabled={page === data.data.totalPages}
        >
          Next
        </button>
      </div>
    </div>
  );
};