import axios from "axios";

const API_URL = "http://localhost:5291/api/Tree";

export const createNode = async (data: {
	title: string;
	description: string;
	parentId?: string;
}) => axios.post(`${API_URL}`, data);

export const deleteNode = async (nodeId: string) =>
	axios.delete(`${API_URL}/${nodeId}`);

export const updateNode = async (
	nodeId: string,
	data: { title: string; description: string }
) => axios.patch(`${API_URL}/${nodeId}`, data);

export const getNodeById = async (nodeId: string) =>
	axios.get(`${API_URL}/${nodeId}`);

export const getAllNodesById = async (nodeId: string) =>
	axios.get(`${API_URL}/${nodeId}/childrens`);
