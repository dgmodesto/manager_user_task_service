import HeaderContent from '@/components/HeaderContent';
import LogoutButton from '@/components/LogoutButton';
import TasksTable from '@/components/TaskTable';
import { getServerSession } from 'next-auth'
import { redirect } from 'next/navigation';
import { Suspense } from 'react';

export default async function Task() {

  const session = await getServerSession();

  if (!session) {
    redirect('/')
  }


  return (
    <div>

      <Suspense fallback={<div>Loading...</div>}>
        <HeaderContent />
        <TasksTable />
      </Suspense>
    </div>)
}