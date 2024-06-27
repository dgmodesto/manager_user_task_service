import HeaderContent from '@/components/HeaderContent';
import NewTaskForm from '@/components/NewTaskForm';
import UpdateTaskForm from '@/components/UpdateTaskForm';
import { getServerSession } from 'next-auth';
import { redirect } from 'next/navigation';
import { Suspense } from 'react';

export default async function UpdateTask({ params }: { params: { id: string } }) {
  const session = await getServerSession();

  if (!session) {
    redirect('/')
  }

  return (
    <div>
      <Suspense fallback={<div>Loading...</div>}>
        <HeaderContent />
        <UpdateTaskForm params={params} />
      </Suspense>
    </div>
  )
}