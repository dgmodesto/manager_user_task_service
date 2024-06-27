"use server";

import { cookies } from "next/headers";

const urlListRealms = `${process.env.API_URL}/UserTask`

export default async function GetUserTask(id: any): Promise<any | null> {

  const bearerToken = cookies().get("token-jwt")?.value;

  const url = `${urlListRealms}/${id}`;

  const response = await fetch(
    url,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${bearerToken}`
      },
      next: {
        tags: ["get-user-task"]
      }
    }
  );

  if (!response?.ok) return null;

  return await response.json();
}
